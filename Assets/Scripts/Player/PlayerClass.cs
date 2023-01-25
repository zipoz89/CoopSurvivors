using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerClass
{
    public PlayerClassType ClassType;


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

    public abstract void CastSkill1(bool state);

    public abstract void CastSkill2(bool state);

    public enum PlayerClassType
    {
        Marine,
        Medium,
    }
    
    public static PlayerClass CreatePlayerClass(PlayerClass.PlayerClassType type)
    {
        switch (type)
        {
            case PlayerClass.PlayerClassType.Marine:
                return new MarineClass();
            case PlayerClass.PlayerClassType.Medium:
                return new MediumClass();
            default:
                return new MarineClass();
        }
    }
}
