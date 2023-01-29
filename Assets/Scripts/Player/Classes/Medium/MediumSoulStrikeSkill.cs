using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumSoulStrikeSkill : NetworkBehaviour,IOnlinePoolable
{
    public void OnGenerated()
    {
        Debug.Log("MediumSoulStrikeSkill generated");
    }

    public void OnSpawned()
    {
        Debug.Log("MediumSoulStrikeSkill spawned");
    }

    public void OnReturned()
    {
        Debug.Log("MediumSoulStrikeSkill returned");
    }
}
