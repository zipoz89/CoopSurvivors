using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace Old
{
    
}
public class EnemyController : NetworkBehaviour
{
    private Transform target;
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius = 10f;
}
