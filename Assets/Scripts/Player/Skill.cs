using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Skill : INetworkPoolableObject
{
    public Action<bool> SkillResetCooldown;
    public Action<Skill> SkillFinished;
    
    [FormerlySerializedAs("colliderObject")] [SerializeField] protected GameObject graphicObject;

    public override void OnGenerated()
    {
        graphicObject.SetActive(false);
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
        graphicObject.SetActive(false);
    }

    public override void OnSpawned()
    {
        graphicObject.SetActive(true);
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
        graphicObject.SetActive(true);
    }

    public override void OnReturned()
    {
        graphicObject.SetActive(false);
        OnReturnedServer();
    }
    
    [ServerRpc]
    protected void OnReturnedServer()
    {
        OnReturnedClient();
    }
    
    [ObserversRpc]
    protected virtual void OnReturnedClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        graphicObject.SetActive(false);
    }
}