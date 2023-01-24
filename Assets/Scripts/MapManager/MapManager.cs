using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

public class MapManager : NetworkBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

    //flags
    private bool gameStarted = false;
    
    //variables
    //[SyncVar(Channel = Channel.Unreliable, OnChange = nameof(on_roundChange))]
    
    [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(OnRoundChange))]
    private int round = 0;
    
    
    [ServerRpc(RequireOwnership = false)]
    public void StartGame()
    {
        if (gameStarted)
        {
            return;
        }
        
        gameStarted = true;

        enemyManager.InitializePool();
        
        round = 1;
        Debug.Log("Game Started");
    }
    
    private void Update()
    {
        
    }
    
    private void OnRoundChange(int prev, int next, bool asServer)
    {
         Debug.Log("Round " + next + " started");
    }
}
