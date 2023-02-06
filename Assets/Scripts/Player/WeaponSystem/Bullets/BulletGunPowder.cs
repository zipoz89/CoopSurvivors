using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletGunPowder : BulletPart
{
    [SerializeField] private float speed;
    public float Speed => speed;
}
