using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : NetworkBehaviour, IOnlinePoolable
{
    public Action<bool> SkillResetCooldown;
    public Action SkillFinished;

    public virtual void OnGenerated()
    {
        OnGeneratedClient();
    }

    [ObserversRpc]
    protected virtual void OnGeneratedClient()
    {
    }

    public virtual void OnSpawned()
    {
        OnSpawnedClient();
    }
    
    [ObserversRpc]
    protected virtual void OnSpawnedClient()
    {
    }

    public virtual void OnReturned()
    {
        OnReturnedClient();
    }
    
    [ObserversRpc]
    protected virtual void OnReturnedClient()
    {
    }
}
