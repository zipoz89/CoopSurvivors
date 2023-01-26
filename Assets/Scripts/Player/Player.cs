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
    public PlayerClass PlayerClass;

    private Camera _playerCamera;

    [SerializeField] private PlayerClassType playerClass;

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (base.IsOwner)
        {
            RegisterPlayerServer(base.Owner, this);

            RegisterInput();
            ChangeClass(PlayerClassType.Marine);
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
        playerMovement.RegisterInput(GameManager.Instance.PlayerInput);
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
    
    public void ChangeClass(PlayerClassType type)
    {
        PlayerClass?.UnregisterInput(GameManager.Instance.PlayerInput);
        
        ChangeClassServer(base.Owner,type);
    }

    [ServerRpc]
    private void ChangeClassServer(NetworkConnection conn,PlayerClassType type)
    {
        //despawn
        PlayerClass?.Despawn();
        
        //spawn new
        GameObject playerClass = Instantiate(GameManager.Instance.PlayerClassDatabase.GetClassPrefab(type));
        playerClass.transform.parent = this.transform;
        base.Spawn(playerClass.gameObject, base.Owner);

        ChangeClassClient(base.Owner,playerClass.GetComponent<PlayerClass>());
    }

    [TargetRpc]
    private void ChangeClassClient(NetworkConnection conn,PlayerClass classObject)
    {
        PlayerClass = classObject;
        PlayerClass.RegisterInput(GameManager.Instance.PlayerInput);
    }

    [SerializeField] private List<IInteractable> players = new List<IInteractable>();

    public void RegisterInput()
    {
        GameManager.Instance.PlayerInput.onInteract += Interact;
    }

    private void Interact(bool state)
    {
        if (state == true)
        {
            players[0]?.Interact(this);
        }

    }

    
    
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("added to interactable list");
            players.Add(other.GetComponent<IInteractable>());
        }
    }
    
    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("removed to interactable list");
            players.Remove(other.GetComponent<IInteractable>());
        }
    }
}
