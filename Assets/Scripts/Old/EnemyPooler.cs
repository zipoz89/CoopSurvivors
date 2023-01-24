using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

namespace Old
{

    public class EnemyPooler : NetworkBehaviour
    {
        [SerializeField] private int initialPoolSize = 40;
        [SerializeField] private int maxPoolSize = 40;
        private int currentPoolSize = 0;

        public int CurrentPoolSize
        {
            get => currentPoolSize;
        }

        [SerializeField] private GameObject enemyPrefab;

        [SerializeField] private bool IsExpandable = true;

        private Queue<TestEnemy> pool = new Queue<TestEnemy>();
        private List<TestEnemy> activeObjects = new List<TestEnemy>();

        public List<TestEnemy> ActiveObjects => activeObjects;

        private int generatedCount = 0;

        // public override void OnStartClient()
        // {
        //     base.OnStartClient();
        //
        //
        // }

        public void InitializePooler()
        {
            Debug.Log("Going to spawn " + initialPoolSize);

            for (int i = 0; i < initialPoolSize; i++)
            {
                Generate();
            }
        }



        // <summary>
        // takes one object from pool, generates new object if able, if not return null
        // </summary>
        public TestEnemy Get()
        {
            TestEnemy obj = TryTake();

            if (obj == null && currentPoolSize < maxPoolSize && IsExpandable)
            {
                //Debug.Log("o to robi ale czemu " + (obj == null) + " " + (currentPoolSize < maxPoolSize));
                Generate();

                obj = TryTake();
            }

            if (obj != null)
            {
                obj.OnSpawned();
            }

            return obj;
        }

        // tries to take one object from pool
        private TestEnemy TryTake()
        {
            TestEnemy result = default(TestEnemy);

            if (pool.Count > 0)
            {
                result = pool.Dequeue();
                activeObjects.Add(result);
            }

            return result;
        }

        public void Return(TestEnemy obj)
        {
            activeObjects.Remove(obj);
            obj.OnReturned();
            pool.Enqueue(obj);
        }

        private void Generate()
        {
            currentPoolSize++;
            GameObject o = GameObject.Instantiate(enemyPrefab, new Vector3(1000, 1000, 0), quaternion.identity);
            ServerManager.Spawn(o);
            var enemy = o.GetComponent<TestEnemy>();
            enemy.OnGenerated(generatedCount++);
            pool.Enqueue(enemy);
            currentPoolSize++;
        }
    }
}