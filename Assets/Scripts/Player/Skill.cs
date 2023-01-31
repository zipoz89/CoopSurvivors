using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : INetworkPoolableObject
{
    public Action<bool> SkillResetCooldown;
    public Action<Skill> SkillFinished;
    
    [SerializeField] protected GameObject colliderObject;

    public override void OnGenerated()
    {
        colliderObject.SetActive(false);
        OnGeneratedServer();
    }

    [ServerRpc]
    protected void OnGeneratedServer()
    {
        OnGeneratedClient();
    }
    
    [ObserversRpc]
    protected void OnGeneratedClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        colliderObject.SetActive(false);
    }

    public override void OnSpawned()
    {
        colliderObject.SetActive(true);
        OnSpawnedServer();
    }
    
    [ServerRpc]
    protected void OnSpawnedServer()
    {
        OnSpawnedClient();
    }
    
    [ObserversRpc]
    protected void OnSpawnedClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        colliderObject.SetActive(true);
    }

    public override void OnReturned()
    {
        colliderObject.SetActive(false);
        OnReturnedServer();
    }
    
    [ServerRpc]
    protected void OnReturnedServer()
    {
        OnReturnedClient();
    }
    
    [ObserversRpc]
    protected void OnReturnedClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        colliderObject.SetActive(false);
    }
}
