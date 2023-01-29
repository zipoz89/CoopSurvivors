using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumClass : PlayerClass
{
    private OnlinePooler<MediumReapSkill> reapPool = new OnlinePooler<MediumReapSkill>();
    private OnlinePooler<MediumSoulStrikeSkill> soulStrikPool = new OnlinePooler<MediumSoulStrikeSkill>();

    [SerializeField] private GameObject reapSkillPrefab;
    [SerializeField] private GameObject soulStrikeSkillPrefab;
    
    public override void InitializeClass()
    {
        reapPool.onObjectSpawnedDestroyed += SpawnDestroySkill;
        soulStrikPool.onObjectSpawnedDestroyed += SpawnDestroySkill;
        
        reapPool.InitializePool(reapSkillPrefab);
        soulStrikPool.InitializePool(soulStrikeSkillPrefab);
    }

    public override void DeinitializeClass()
    {
        reapPool.DeinitializePool();
        soulStrikPool.DeinitializePool();
        
        reapPool.onObjectSpawnedDestroyed -= SpawnDestroySkill;
        soulStrikPool.onObjectSpawnedDestroyed -= SpawnDestroySkill;
    }

    public override void CastSkill1(bool state)
    {
        if (state)
        {
            StartCoroutine(reapSkill());
            
            Debug.Log("Cast medium skill 1!",this);
        }
    }

    IEnumerator reapSkill()
    {
        var reap = reapPool.Get();

        float elapsedTime = 0;

        reap.transform.parent = this.transform;
        reap.transform.localPosition = Vector3.zero;

        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            //reap.transform.position = this.transform.position;
            yield return null;
        }

        reap.transform.parent = null;
        
        reapPool.Return(reap);
    }

    public override void CastSkill2(bool state)
    {
        if (state)
        {
            Debug.Log("Cast medium skill 2!",this);
        }
    }

    private void SpawnDestroySkill(IOnlinePoolable o,bool mode)
    {
        if (mode == true)
        {
            ServerManager.Spawn(((NetworkBehaviour)o).gameObject);
        }
        else
        {
            ServerManager.Despawn(((NetworkBehaviour)o).gameObject);
        }

    }
}
