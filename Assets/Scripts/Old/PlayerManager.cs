using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Old
{

    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        private static PlayerManager instance = null;
        public Action<NetworkConnection> onPlayerConnected;

        private Dictionary<NetworkConnection, Player> players = new Dictionary<NetworkConnection, Player>();


        

        [Server]
        public void RegisterPlayer(NetworkConnection connection, Player player)
        {
            players[connection] = player;
            onPlayerConnected?.Invoke(connection);
        }


        [Server]
        public Transform GetRandomPlayerTransform()
        {
            var players = this.players.Values.ToArray();

            return players[Random.Range(0, players.Length)].transform;
        }

        [Server]
        public Transform GetClosestPlayerTransform(Vector2 pos)
        {
            var playerArray = players.Values.ToArray();

            int closestPlayerIndex = 0;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < playerArray.Length; i++)
            {
                float distance = Vector2.Distance(playerArray[i].transform.position, pos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayerIndex = i;
                }
            }

            return playerArray[closestPlayerIndex].transform;
        }
    }
}