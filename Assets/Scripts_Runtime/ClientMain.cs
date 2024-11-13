using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;
using UnityProtocol;

namespace UnityClient {

    public class ClientMain : MonoBehaviour {

        Telepathy.Client client;

        bool isTearDown;

        void Start() {
            int messageSize = 1024;
            client = new Client(messageSize);

            // - Binding Events
            client.OnConnected = () => {
                Debug.Log("Connected to the server");
            };

            client.OnData = (ArraySegment<byte> data) => {
                Debug.Log("Received a message from the server: " + data.Count);
            };

            client.OnDisconnected = () => {
                Debug.Log("Disconnected from the server");
            };

            client.Connect("localhost", 7777);

            Debug.Log("Press Space to connect/disconnect");

        }

        void Update() {

            if (client.Connected) {
                if (Input.GetKeyUp(KeyCode.Q)) {

                    LoginMessage message = new LoginMessage();
                    message.username = "chenwan";

                    byte[] data = BakeMessage(1, message);
                    client.Send(data);

                } else if (Input.GetKeyUp(KeyCode.W)) {
                    HelloMessage message = new HelloMessage();
                    message.myName = "CW";
                    message.myAge = 18;
                    message.myData = "Hello Server!";

                    byte[] data = BakeMessage(2, message);
                    client.Send(data);
                }
            }

            client.Tick(100);

        }

        byte[] BakeMessage<T>(int headerID, T message) where T : struct {
            // 1. struct Message -> string
            string jsonStr = JsonUtility.ToJson(message);
            // 2. string -> byte[]
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonStr);

            // 3. byte[] -> byte[] (add header)
            byte[] final = new byte[data.Length + 4];
            final[0] = (byte)headerID;
            final[1] = (byte)(data.Length >> 8);
            final[2] = (byte)(data.Length >> 16);
            final[3] = (byte)(data.Length >> 24);

            Array.Copy(data, 0, final, 4, data.Length);
            return final;
        }

        void OnDestroy() {
            OnTearDown();
        }

        void OnApplicationQuit() {
            OnTearDown();
        }

        void OnTearDown() {
            if (isTearDown) {
                return;
            }
            isTearDown = true;
            client.Disconnect();
        }
    }
}
