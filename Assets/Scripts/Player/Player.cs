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
    
    [SerializeField] private List<IInteractable> interactables = new List<IInteractable>();

    #region SetUp
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        if (base.IsOwner)
        {
            RegisterPlayerServer(base.Owner, this);

            RegisterInput();
            ChangeClass(PlayerClassType.Medium);
            SetUpCamera();
            SetUpMovement();
        }
        else
        {
            playerMovement.enabled = false;
        }
    }
    
    public void RegisterInput()
    {
        GameManager.Instance.PlayerInput.onInteract += Interact;
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
    #endregion

    #region class
    public void ChangeClass(PlayerClassType type)
    {
        PlayerClass?.UnregisterInput(GameManager.Instance.PlayerInput);
        PlayerClass?.DeinitializeClass();
        
        ChangeClassServer(base.Owner,type);
    }

    [ServerRpc]
    private void ChangeClassServer(NetworkConnection conn,PlayerClassType type)
    {
        //despawn
        
        PlayerClass?.Despawn();
        
        //spawn new
        GameObject playerClassObject = Instantiate(GameManager.Instance.PlayerClassDatabase.GetClassPrefab(type));
        playerClassObject.transform.parent = this.transform;
        playerClassObject.transform.localPosition = Vector3.zero;
        base.Spawn(playerClassObject.gameObject, base.Owner);

        var playerClass = playerClassObject.GetComponent<PlayerClass>();

        ChangeClassClient(base.Owner,playerClass);
    }

    [TargetRpc]
    private void ChangeClassClient(NetworkConnection conn,PlayerClass classObject)
    {
        
        classObject.InitializeClass();
        PlayerClass = classObject;
        PlayerClass.RegisterInput(GameManager.Instance.PlayerInput);
    }

    #endregion

    private void Interact(bool state)
    {
        if (state == true)
        {
            interactables[0]?.Interact(this);
        }

    }
    
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("added to interactable list");
            interactables.Add(other.GetComponent<IInteractable>());
        }
    }
    
    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            //Debug.Log("removed to interactable list");
            interactables.Remove(other.GetComponent<IInteractable>());
        }
    }
}
