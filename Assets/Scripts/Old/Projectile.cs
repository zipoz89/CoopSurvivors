using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace Old
{

    public class Projectile : NetworkBehaviour
    {
        [SerializeField] public float speed = 4;
        [SerializeField] public float maxLife = 5;
        private float elapsedTime = 0;

        [SerializeField] private GameObject grahpics;

        private void Update()
        {
            HandleLife();
            HandleMove();
        }

        private void HandleLife()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > maxLife)
            {
                ServerManager.Despawn(this.gameObject);
            }
        }

        private void HandleMove()
        {
            this.transform.position += transform.right * speed * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent(out IDamagable damagable))
            {
                grahpics.SetActive(false);
                damagable.Damage(10f);
                ServerManager.Despawn(this.gameObject);
            }
        }
    }
}
