using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public String Prompt { get; set; }
    public bool Interact(Player player);
}
