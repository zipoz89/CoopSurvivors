using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private ServerManager _serverManager;

    public Action<NetworkConnection> onPlayerConnected;
    public Action<NetworkConnection> onPlayerDisconnected;

    private Dictionary<NetworkConnection, Player> connectedPlayers = new Dictionary<NetworkConnection, Player>();

    public int PlayersLoadedOnMap { get; private set; } = 0;
    public int PlayerCount => connectedPlayers.Count;
    

    private void Awake()
    {
        _serverManager.OnRemoteConnectionState += OnServerConnectionState;
    }

    private void OnServerConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == RemoteConnectionState.Stopped)
        {
            connectedPlayers.Remove(connection);
            onPlayerDisconnected?.Invoke(connection);
        }
    }

    
    public void RegisterPlayer(NetworkConnection connection, Player player)
    {
        connectedPlayers[connection] = player;
        onPlayerConnected?.Invoke(connection);
    }

    
    public void SetPlayerLoadedOnMap()
    {
        PlayersLoadedOnMap++;
    }
    

    public NetworkObject[] GetPlayersNetworkObjects()
    {
        var players = connectedPlayers.Values.ToArray();
        NetworkObject[] res = new NetworkObject[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            res[i] = players[i].GetComponent<NetworkObject>();
        }

        return res;
    }


    public Transform GetRandomPlayerTransform()
    {
        var players = this.connectedPlayers.Values.ToArray();

        return players[Random.Range(0, players.Length)].transform;
    }


    public Transform GetClosestPlayerTransform(Vector2 pos)
    {
        var playerArray = connectedPlayers.Values.ToArray();

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
