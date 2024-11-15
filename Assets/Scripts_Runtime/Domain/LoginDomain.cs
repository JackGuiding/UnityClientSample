using System;
using UnityEngine;
using UnityProtocol;

namespace UnityClient {

    public static class LoginDomain {

        public static void SendLogin(ClientContext ctx, string username) {
            LoginReqMessage msg = new LoginReqMessage();
            msg.username = username;

            byte[] data = MessageHelper.EncodeMessage(msg);
            ctx.client.Send(data);
        }

        public static void OnData(ClientContext ctx, ArraySegment<byte> data) {
            int headerID = BitConverter.ToInt32(data.Array, 0);
            string jsonStr = System.Text.Encoding.UTF8.GetString(data.Array, 4, data.Count - 4);

            if (headerID == MessageHelper.LOGIN_RES) {
                LoginResMessage msg = JsonUtility.FromJson<LoginResMessage>(jsonStr);
                OnLoginRes(ctx, msg);
            } else if (headerID == MessageHelper.LOGIN_BROADCAST) {
                LoginBroadcastMessage msg = JsonUtility.FromJson<LoginBroadcastMessage>(jsonStr);
                OnLoginBroadcast(ctx, msg);
            } else {
                Debug.LogError("Unknown headerID: " + headerID);
            }
        }

        static void OnLoginRes(ClientContext ctx, LoginResMessage msg) {
            Debug.Log("LoginRes: " + msg.status);
        }

        static void OnLoginBroadcast(ClientContext ctx, LoginBroadcastMessage msg) {
            GameObject go = GameObject.Instantiate(ctx.playerEntityPrefab);
            PlayerEntity player = go.GetComponent<PlayerEntity>();
            player.username = msg.username;
            player.SetPos(msg.pos);

            ctx.playerRepository.Add(player);
        }

    }

}
