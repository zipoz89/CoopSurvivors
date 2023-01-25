using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumClass : PlayerClass
{
    public MediumClass()
    {
        ClassType = PlayerClassType.Medium;
    }
    
    public override void CastSkill1(bool state)
    {
        if (state)
        {
            Debug.Log("Cast medium skill 1!");
        }
    }

    public override void CastSkill2(bool state)
    {
        if (state)
        {
            Debug.Log("Cast medium skill 2!");
        }
    }
}
