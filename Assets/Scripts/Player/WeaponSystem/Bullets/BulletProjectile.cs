using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletProjectile : BulletPart
{
    [SerializeField] private float damage;
    public float Damage => damage;
}
