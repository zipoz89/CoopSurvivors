using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCharacter playerCharacter;
    public PlayerClass PlayerClass { get; private set; }

    private Camera _playerCamera;

    [SerializeField] private PlayerClass.PlayerClassType playerClass;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (base.IsOwner)
        {
            RegisterPlayerServer(base.Owner, this);

            ChangeClass(new MarineClass());
            SetUpCamera();
            SetUpMovement();
            SetUpCharacter();
        }
        else
        {
            playerMovement.enabled = false;
            playerCharacter.enabled = false;
        }
    }

    private void SetUpMovement()
    {
        playerMovement.RegisterInput(GameManager.Instance.PlayerInput);
    }

    private void SetUpCharacter()
    {
        playerCharacter.RegisterInput(GameManager.Instance.PlayerInput, this);
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
    
    public void ChangeClass(PlayerClass playerClass)
    {
        Debug.Log("Changing class to " + playerClass.ClassType + " from " + (PlayerClass != null ? PlayerClass.ClassType : "none"));
        
        this.PlayerClass?.UnregisterInput(GameManager.Instance.PlayerInput);
        this.PlayerClass = playerClass;
        this.PlayerClass.RegisterInput(GameManager.Instance.PlayerInput);
        ChangeClassOnServer(playerClass.ClassType);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeClassOnServer(PlayerClass.PlayerClassType classToChangeInto)
    {
        if (!base.IsServer)
        {
            this.PlayerClass =  PlayerClass.CreatePlayerClass(classToChangeInto);
        }

        playerClass = classToChangeInto;
    }

    
}
