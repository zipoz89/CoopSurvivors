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

    private bool reapIsCasting = false;
    
    public override void CastSkill1(bool state)
    {
        if (!reapIsCasting && state)
        {
            StartReap();
        }
        else if (!state)
        {
            StopReap();
        }
    }

    private MediumReapSkill reap;
    
    private void StartReap()
    {
        reap = reapPool.Get();
        reap.SkillFinished += ReturnReap;
        reap.transform.parent = this.transform;
        reap.transform.localPosition = Vector3.zero;
        reap.StartReap(4);//TODO: add to stat system 
    }

    private void StopReap()
    {
        reap.ApplyReap();
    }

    private void ReturnReap()
    {
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


}
