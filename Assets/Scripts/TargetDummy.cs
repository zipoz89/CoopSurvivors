using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class TargetDummy : NetworkBehaviour, IDamagable, IKnockbackable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float recivedDamage = 0;


    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public float DealDamage(float damagePoints, DamageType type, NetworkConnection player)
    {
        recivedDamage += damagePoints;
        DealDamageOnServer(damagePoints, type, player);
        return damagePoints;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DealDamageOnServer(float damagePoints, DamageType type, NetworkConnection player)
    {
        recivedDamage += damagePoints;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DealDamageOnClient(float damagePoints, DamageType type, NetworkConnection player)
    {
        recivedDamage += damagePoints;
    }

    public void ApplyKnockBack(float knockBackForce, Vector2 direction)
    {
        rb.AddForce(direction*knockBackForce);
    }
}
