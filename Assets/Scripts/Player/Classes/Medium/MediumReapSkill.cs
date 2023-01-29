using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumReapSkill : NetworkBehaviour,IOnlinePoolable
{
    [SerializeField] private GameObject graphicsObject;
    
    public void OnGenerated()
    {
        Debug.Log("MediumReapSkill generated");
        OnGeneratedClient();
    }

    [ObserversRpc]
    private void OnGeneratedClient()
    {
        graphicsObject.SetActive(false);
    }

    public void OnSpawned()
    {
        Debug.Log("MediumReapSkill spawned");
        OnSpawnedClient();
    }
    
    [ObserversRpc]
    private void OnSpawnedClient()
    {
        graphicsObject.SetActive(true);
    }

    public void OnReturned()
    {
        Debug.Log("MediumReapSkill returned");
        OnReturnedClient();
    }
    [ObserversRpc]
    private void OnReturnedClient()
    {
        graphicsObject.SetActive(false);
    }
}
