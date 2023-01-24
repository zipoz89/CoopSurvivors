using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FishNet.Connection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Old
{

    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance = null;
        public Action<NetworkConnection> onPlayerConnected;

        private Dictionary<NetworkConnection, Player> players = new Dictionary<NetworkConnection, Player>();

        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
                }

                return instance;
            }
        }

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void RegisterPlayer(NetworkConnection connection, Player player)
        {
            players[connection] = player;
            onPlayerConnected?.Invoke(connection);
        }

        public Transform GetRandomPlayerTransform()
        {
            var players = this.players.Values.ToArray();

            return players[Random.Range(0, players.Length)].transform;
        }

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