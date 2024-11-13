using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;

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

            if (Input.GetKeyUp(KeyCode.Space)) {
                if (client.Connected) {
                    client.Send(new ArraySegment<byte>(new byte[1] { 0 }));
                    Debug.Log("Connecting to the server");
                }
            }

            client.Tick(100);

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
