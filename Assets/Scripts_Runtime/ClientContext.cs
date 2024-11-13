using System;

namespace UnityClient {

    public class ClientContext {

        public Telepathy.Client client;

        public ClientContext() { }

        public void Inject(Telepathy.Client client) {
            this.client = client;
        }
    }
}