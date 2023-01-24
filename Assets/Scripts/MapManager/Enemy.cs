using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    public void OnGenerated()
    {
        Debug.Log("Hello world!");
    }

    public void OnSpawned()
    {
        throw new System.NotImplementedException();
    }

    public void OnReturned()
    {
        throw new System.NotImplementedException();
    }
}
