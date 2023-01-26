using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineClass : PlayerClass
{
    public override void CastSkill1(bool state)
    {
        if (state)
        {
            Debug.Log("Cast marine skill 1!",this);
        }
    }

    public override void CastSkill2(bool state)
    {
        if (state)
        {
            Debug.Log("Cast marine skill 2!",this);
        }
    }
}
