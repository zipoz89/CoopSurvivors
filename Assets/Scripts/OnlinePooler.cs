using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Managing.Server;
using UnityEngine;

public class OnlinePooler<T> where T : IOnlinePoolable
{
    private GameObject objectPrefab;
    
    int initialPoolSize = 1;
    int maxPoolSize = 10;
    
    public int CurrentPoolSize { get; private set; } = 0;
    
    private Queue<T> pool = new Queue<T>();
    private List<T> activeObjects = new List<T>();
    
    public List<T> ActiveObjects => activeObjects;

    public Action<T,bool> onObjectSpawnedDestroyed;
    
    public void InitializePool(GameObject prefab)
    {
        objectPrefab = prefab;
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            Generate();
        }
    }

    public void DeinitializePool()
    {
        List<T> all = pool.ToList();
        all = all.Concat(activeObjects).ToList();

        foreach (var t in all)
        {
            onObjectSpawnedDestroyed?.Invoke(t,false);
        }
    }

    public T Get()
    {
        T obj = TryTake();

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
    
    private T TryTake()
    {
        T result = default(T);

        if (pool.Count > 0)
        {
            result = pool.Dequeue();
            activeObjects.Add(result);
        }

        return result;
    }

    public void Return(T obj)
    {
        activeObjects.Remove(obj);
        obj.OnReturned();
        pool.Enqueue(obj);
    }
    
    private void Generate()
    {
        CurrentPoolSize++;
        GameObject o = GameObject.Instantiate(objectPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
        //ServerManager.Spawn(o);
        var pollableScript = o.GetComponent<T>();
        onObjectSpawnedDestroyed?.Invoke(pollableScript,true);
        pollableScript.OnGenerated();
        pool.Enqueue(pollableScript);
    }
}
