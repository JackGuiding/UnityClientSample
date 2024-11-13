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

            if (headerID == MessageHelper.HEADER_LOGIN_RES) {
                string jsonStr = System.Text.Encoding.UTF8.GetString(data.Array, 4, data.Count - 4);
                LoginResMessage msg = JsonUtility.FromJson<LoginResMessage>(jsonStr);
                OnLoginRes(ctx, msg);
            }
        }

        static void OnLoginRes(ClientContext ctx, LoginResMessage msg) {
            Debug.Log("LoginRes: " + msg.status);
        }

    }

}
