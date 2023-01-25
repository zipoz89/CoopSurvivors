using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassChanger : MonoBehaviour, IInteractable
{
    [SerializeField] private String prompt;
    [SerializeField] private PlayerClass.PlayerClassType classToChangeInto;
    public string Prompt { get; set; }
    
    public bool Interact(Player player)
    {
        Debug.Log(classToChangeInto + " a ma: " + player.PlayerClass.ClassType);
        if (player.PlayerClass.ClassType != classToChangeInto)
        {
            Debug.Log(classToChangeInto);
            player.ChangeClass(PlayerClass.CreatePlayerClass(classToChangeInto));
            return true;
        }
        else
        {
            return false;
        }
    }
    

    
}
