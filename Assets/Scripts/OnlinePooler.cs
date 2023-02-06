using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Managing.Server;
using FishNet.Object;
using UnityEngine;

public class OnlinePooler<T> : NetworkBehaviour where T : INetworkPoolableObject
{
    [SerializeField] private GameObject objectPrefab;
    
    [SerializeField] private int initialPoolSize = 1;
    [SerializeField] private int maxPoolSize = 10;
    
    public int CurrentPoolSize { get; private set; } = 0;
    
    private Queue<T> pool = new Queue<T>();
    private List<T> activeObjects = new List<T>();
    
    public List<T> ActiveObjects => activeObjects;

    private int generating = 0;
    
    public void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            generating++;
            Generate(base.Owner);
        }
    }

    public void DeinitializePool()
    {
        List<T> all = pool.ToList();
        all = all.Concat(activeObjects).ToList();

        foreach (var t in all)
        {
            DestroyObject(t);
        }
    }

    public async UniTask<T>  Get()
    {
        T obj = TryTake();

        if (obj == null && CurrentPoolSize < maxPoolSize)
        {
            generating++;
            Generate(base.Owner);
            
            await UniTask.WaitUntil(() => generating == 0);
            
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
            //Debug.Log("Normalnie dequeue robi?");
            
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
    
    [ServerRpc]
    private void Generate(NetworkConnection nc)
    {
        CurrentPoolSize++;
        GameObject o = Instantiate(objectPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
        o.name = objectPrefab.name + CurrentPoolSize;
        ServerManager.Spawn(o,nc);

        var pollableScript = o.GetComponent<T>();
        pollableScript.Index = CurrentPoolSize - 1;
        
        RegisterOnUser(nc,o);
    }

    [TargetRpc]
    private void RegisterOnUser(NetworkConnection nc,GameObject o)
    {
        var pollableScript = o.GetComponent<T>();
        
        pollableScript.OnGenerated();
        pool.Enqueue(pollableScript);
        generating--;
    }

    [ServerRpc]
    private void DestroyObject(NetworkBehaviour no)
    {
        no.Despawn();
    }
}
