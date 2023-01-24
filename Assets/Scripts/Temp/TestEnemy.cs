using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Old;
using UnityEngine;

public class TestEnemy : NetworkBehaviour, IPoolableObject, IDamagable
{
    public Transform Target { get; set; }
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Collider2D _collider2D;
    
    public Action<TestEnemy> onKilled;

    private float hp;
    
    [ServerRpc(RequireOwnership = false)]
    public void OnGenerated(int objectId)
    {
        OnGeneratedClient(objectId);
    }
    
    [ObserversRpc]
    private void OnGeneratedClient(int objectId)
    {
        this.transform.position = new Vector3(1000, 1000, 0);
        this.gameObject.name = "Enemy " + objectId;
        renderer.enabled = false;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void OnSpawned()
    {
        OnSpawnedClient();
    }
    
    [ObserversRpc]
    private void OnSpawnedClient()
    {
        renderer.enabled = true;
        _collider2D.enabled = true;
        hp = 10;
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnReturned()
    {
        OnReturnedClient();
    }

    [ObserversRpc]
    private void OnReturnedClient()
    {
        renderer.enabled = false;
        _collider2D.enabled = false;
    }
    
    public void CustomUpdate()
    {
        if (Target != null)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 playerDir = (Target.position - this.transform.position).normalized;
        transform.position += (Vector3)playerDir * (moveSpeed * Time.deltaTime);
    }

    public void Damage(float damage)
    {
        DamageOnServer(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DamageOnServer(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            onKilled?.Invoke(this);
        }
    }
}
