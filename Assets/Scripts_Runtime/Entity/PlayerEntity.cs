using System;
using UnityEngine;

namespace UnityClient {

    public class PlayerEntity : MonoBehaviour {

        public string username;

        public void Ctor() {

        }

        public void SetPos(Vector2 pos) {
            transform.position = pos;
        }

    }

}