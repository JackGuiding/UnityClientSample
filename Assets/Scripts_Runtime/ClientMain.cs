using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;
using UnityProtocol;

namespace UnityClient {

    public class ClientMain : MonoBehaviour {

        ClientContext ctx;

        [SerializeField] GameObject playerEntityPrefab;

        Telepathy.Client client;

        bool isTearDown;

        void Start() {

            // ==== Ctor ====
            ctx = new ClientContext();
            int messageSize = 1024;
            client = new Client(messageSize);

            // ==== Inject ====
            ctx.Inject(client);
            ctx.playerEntityPrefab = playerEntityPrefab;

            // - Binding Events
            client.OnConnected = () => {
                Debug.Log("Connected to the server");
            };

            client.OnData = (ArraySegment<byte> data) => {
                LoginDomain.OnData(ctx, data);
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
                    LoginDomain.SendLogin(ctx, "cyl");
                } else if (Input.GetKeyUp(KeyCode.W)) {
                    HelloReqMessage message = new HelloReqMessage();
                    message.myName = "CW";
                    message.myAge = 18;
                    message.myData = "Hello Server!";

                    byte[] data = MessageHelper.EncodeMessage(message);
                    client.Send(data);
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
