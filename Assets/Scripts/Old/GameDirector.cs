using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Old
{

    public class GameDirector : NetworkBehaviour
    {
        [SerializeField] private EnemyPooler enemyPooler;

        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(on_roundChange))]
        private int round = 0;

        [SerializeField] private float spawnRate = 1;
        [SerializeField] private float spawnRadius = 10f;
        private float elapsedTimeSpawned = 0;

        private bool gameStarted = false;

        [ServerRpc(RequireOwnership = false)]
        public void StartGame()
        {
            if (gameStarted)
            {
                return;
            }

            gameStarted = true;

            enemyPooler.InitializePooler();
            round = 1;
            Debug.Log("Game Started");
        }

        private void Update()
        {
            if (round > 0)
            {
                ManageSpawnTime();
                UpdateEnemies();
            }
        }

        private void UpdateEnemies()
        {
            foreach (var enemy in enemyPooler.ActiveObjects)
            {
                enemy.CustomUpdate();
            }
        }

        private void ManageSpawnTime()
        {
            if (!base.IsServer)
            {
                return;
            }

            elapsedTimeSpawned += Time.deltaTime;
            if (elapsedTimeSpawned >= spawnRate)
            {
                elapsedTimeSpawned = 0;
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {


            TestEnemy enemy = enemyPooler.Get();

            if (enemy)
            {
                var randomOnCircle = Random.insideUnitCircle.normalized * spawnRadius;

                var randomPlayer = PlayerManager.Instance.GetRandomPlayerTransform();
                var pos = randomPlayer.position + (Vector3)randomOnCircle;

                SetEnemySpawnDataServer(enemy, randomPlayer, pos);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetEnemySpawnDataServer(TestEnemy enemy, Transform target, Vector3 pos)
        {
            SetEnemySpawnDataClients(enemy, target, pos);
        }

        [ObserversRpc(RunLocally = true)]
        private void SetEnemySpawnDataClients(TestEnemy enemy, Transform target, Vector3 pos)
        {
            if (base.IsServer)
            {
                enemy.onKilled += OnEnemyKilled;
            }

            enemy.transform.position = pos;
            enemy.Target = target;
        }

        private void on_roundChange(int prev, int next, bool asServer)
        {
            Debug.Log("Round " + next);
        }

        private void OnEnemyKilled(TestEnemy enemy)
        {
            Debug.Log("enemy killed");
            enemyPooler.Return(enemy);
        }
    }
}