using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerClass : NetworkBehaviour
{
    public PlayerClassType ClassType;

    protected Vector2 lookDir;
    
    public virtual void InitializeClass(){}

    public virtual void DeinitializeClass(){}

    public void UnregisterInput(PlayerInput input)
    {
        input.onSkill1 -= CastSkill1;
        input.onSkill2 -= CastSkill2;
    }

    public void RegisterInput(PlayerInput input)
    {
        input.onSkill1 += CastSkill1;
        input.onSkill2 += CastSkill2;
    }

    protected abstract void CastSkill1(bool state);

    protected abstract void CastSkill2(bool state);

    public void SetLookDir(Vector2 lookDir)
    {
        this.lookDir = lookDir;
    }

}
