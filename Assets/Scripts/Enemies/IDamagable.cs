using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine;

public interface IDamagable
{
    public GameObject GetGameObject();
    
    public float DealDamage(float damagePoints, DamageType type, NetworkConnection player);

}

public enum DamageType
{
    GunShoot,
    Magic,
    Melee ,
}