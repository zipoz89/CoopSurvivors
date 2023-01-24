using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    
    private Camera _playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        
        if (base.IsOwner)
        {
            RegisterPlayerServer(base.Owner, this);
            
            SetUpCamera();
            SetUpMovement();
        }
        else
        {
            playerMovement.enabled = false;
        }
    }

    private void SetUpMovement()
    {
        playerMovement.SetUpInput(GameManager.Instance.PlayerInput);
    }

    private void SetUpCamera()
    {
        _playerCamera = Camera.main;
        _playerCamera.transform.parent = playerMovement.transform;
        _playerCamera.transform.localPosition = new Vector3(0,0,-10);
    }

    [ServerRpc]
    public void RegisterPlayerServer(NetworkConnection owner, Player player)
    {
        GameManager.Instance.PlayerManager.RegisterPlayer(owner, player);
    }



}
