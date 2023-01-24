using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float spawnRate = 1;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius = 10f;
    private ObjectPooler<TestEnemy> enemyPool;
    
    
    
    private float elapsedTimeSpawned = 0;

    private void Start()
    {
        enemyPool = new ObjectPooler<TestEnemy>();
        enemyPool.Initialize(enemyPrefab);
    }

    private void Update()
    {
        ManageSpawnTime();
        UpdateEnemies();
    }

    private void UpdateEnemies()
    {
        foreach (var enemy in enemyPool.ActiveObjects)
        {
            Debug.Log(enemy.gameObject.name);
            enemy.CustomUpdate();
        }
    }

    private void ManageSpawnTime()
    {
        elapsedTimeSpawned += Time.deltaTime;
        if (elapsedTimeSpawned >= spawnRate)
        {
            elapsedTimeSpawned = 0;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        TestEnemy enemy = enemyPool.Get();

        if (enemy)
        {
            var randomOnCircle = Random.insideUnitCircle.normalized * spawnRadius;

            enemy.transform.position = target.position + (Vector3)randomOnCircle;
            enemy.Target = target;
        }
    }
    
    
}
