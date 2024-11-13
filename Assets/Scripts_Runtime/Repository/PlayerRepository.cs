using System;
using System.Collections.Generic;

namespace UnityClient {

    public class PlayerRepository {

        Dictionary<string, PlayerEntity> all;

        public PlayerRepository() {
            all = new Dictionary<string, PlayerEntity>();
        }

        public void Add(PlayerEntity player) {
            all.TryAdd(player.username, player);
        }

        public void Remove(string username) {
            all.Remove(username);
        }

        public void Foreach(Action<PlayerEntity> action) {
            foreach (var player in all.Values) {
                action(player);
            }
        }

        public bool TryGet(string username, out PlayerEntity player) {
            return all.TryGetValue(username, out player);
        }
    }
}