using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Old
{

    public class GameManager : NetworkBehaviour
    {
        private StateMachine stateMachine;

        private bool isInitialized = false;


        public override void OnStartServer()
        {
            base.OnStartServer();

            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            stateMachine.Initialize();
            stateMachine.ChangeState(StateMachine.States.LobbyState);

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            stateMachine.UpdateState();
        }

    }
//tutaj fajnie zrobiłem syncowanie danych serwera z kilentem przy dołączeniu ale chyba bez sensu ale zostawie na póxniej jbc
// PlayerManager.Instance.onPlayerConnected += SyncOnClient;
// private void SyncOnClient(NetworkConnection con)
// {
//     SyncDataFromServer(con);
//     SyncDataOnClient(con);
//     Debug.Log("Sync?");
// }
//
// [ServerRpc(RequireOwnership = false)]
// public void SyncDataFromServer(NetworkConnection con)
// {
//     Debug.Log("Sync on server?");
//     SyncDataOnClient(con);
// }
//
// [TargetRpc]
// public void SyncDataOnClient(NetworkConnection con)
// {
//     Debug.Log("State machine ization");
//     InitializeStateMachine();
// }
}