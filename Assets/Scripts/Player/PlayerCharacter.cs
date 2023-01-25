using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private List<IInteractable> players = new List<IInteractable>();

    private Player player;
    
    public void RegisterInput(PlayerInput input,Player player)
    {
        input.onInteract += Interact;
        this.player = player;
    }

    private void Interact(bool state)
    {
        if (state == true)
        {
            players[0]?.Interact(player);
        }

    }

    
    
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("added to interactable list");
            players.Add(other.GetComponent<IInteractable>());
        }
    }
    
    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("removed to interactable list");
            players.Remove(other.GetComponent<IInteractable>());
        }
    }
}
