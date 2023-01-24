using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Old
{

    public class Player : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (base.IsOwner)
            {
                playerController.SetCamera(Camera.main);
                RegisterPlayerServer(base.Owner, this);
            }
            else
            {
                playerController.SetOtherPlayer();
            }
        }

        [ServerRpc]
        public void RegisterPlayerServer(NetworkConnection owner, Player player)
        {
            PlayerManager.Instance.RegisterPlayer(owner, player);
        }
        
    }
}