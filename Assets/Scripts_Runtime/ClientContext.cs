using System;
using UnityEngine;

namespace UnityClient {

    public class ClientContext {

        public Telepathy.Client client;

        public GameObject playerEntityPrefab;

        public PlayerRepository playerRepository;

        public ClientContext() {
            playerRepository = new PlayerRepository();
        }

        public void Inject(Telepathy.Client client) {
            this.client = client;
        }
    }
}