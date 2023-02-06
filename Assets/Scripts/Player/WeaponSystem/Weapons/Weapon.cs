using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [SerializeField] private float ShootingInterval;
    [SerializeField] private int Magazine;
    [SerializeField] private float ReloadSpeed;
    [SerializeField] private int BulletsPerShot;
    [SerializeField] private float RandomSpreadAngle;
}
