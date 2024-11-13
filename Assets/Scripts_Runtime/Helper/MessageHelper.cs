using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityProtocol {

    public static class MessageHelper {

        public const int HEADER_LOGIN_REQ = 10;
        public const int HEADER_LOGIN_RES = 11;
        public const int HEADER_HELLO_REQ = 20;

        static Dictionary<Type, int> typeToHeaderID = new Dictionary<Type, int>() {
            {typeof(LoginReqMessage), HEADER_LOGIN_REQ},
            {typeof(LoginResMessage), HEADER_LOGIN_RES},
            {typeof(HelloReqMessage), HEADER_HELLO_REQ},
        };

        public static byte[] EncodeMessage<T>(T message) where T : struct {
            // 1. struct Message -> string
            string jsonStr = JsonUtility.ToJson(message);
            // 2. string -> byte[]
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

            // 3. byte[] -> byte[] (add header)
            Type type = typeof(T);
            int headerID = typeToHeaderID[type];
            byte[] final = new byte[data.Length + 4];
            final[0] = (byte)headerID;
            final[1] = (byte)(headerID >> 8);
            final[2] = (byte)(headerID >> 16);
            final[3] = (byte)(headerID >> 24);

            Array.Copy(data, 0, final, 4, data.Length);
            return final;
        }

        public static T DecodeMessage<T>(ArraySegment<byte> data) where T : struct {
            int headerID = BitConverter.ToInt32(data.Array, 0);
            if (headerID == HEADER_LOGIN_RES) {
                return DecodeMessageWithoutHeader<T>(data);
            } else {
                throw new Exception("Unknown headerID: " + headerID);
            }
        }

        static T DecodeMessageWithoutHeader<T>(ArraySegment<byte> data) where T : struct {
            // 1. byte[] -> string
            string jsonStr = System.Text.Encoding.UTF8.GetString(data.Array, data.Offset, data.Count);
            // 2. string -> struct Message
            T message = JsonUtility.FromJson<T>(jsonStr);
            return message;
        }

    }

}