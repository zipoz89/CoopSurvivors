using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Server;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private int initialPoolSize = 40;
    [SerializeField] private int maxPoolSize = 300;
    public int CurrentPoolSize { get; private set; } = 0;

    private Queue<Enemy> pool = new Queue<Enemy>();
    private List<Enemy> activeObjects = new List<Enemy>();
    
    public List<Enemy> ActiveObjects => activeObjects;
    

    public void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Generate();
        }
    }
    
    public Enemy Get()
    {
        Enemy obj = TryTake();

        if (obj == null && CurrentPoolSize < maxPoolSize)
        {
            Generate();

            obj = TryTake();
        }

        if (obj != null)
        {
            obj.OnSpawned();
        }

        return obj;
    }
    
    private Enemy TryTake()
    {
        Enemy result = default(Enemy);

        if (pool.Count > 0)
        {
            result = pool.Dequeue();
            activeObjects.Add(result);
        }

        return result;
    }

    public void Return(Enemy obj)
    {
        activeObjects.Remove(obj);
        obj.OnReturned();
        pool.Enqueue(obj);
    }
    
    private void Generate()
    {
        CurrentPoolSize++;
        GameObject o = GameObject.Instantiate(enemyPrefab, new Vector3(1000, 1000, 0), quaternion.identity);
        ServerManager.Spawn(o);
        var enemy = o.GetComponent<Enemy>();
        enemy.OnGenerated();
        pool.Enqueue(enemy);
    }
}
