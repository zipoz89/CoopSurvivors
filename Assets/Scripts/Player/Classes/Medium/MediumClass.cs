using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class MediumClass : PlayerClass
{
    //[SerializeField] private OnlinePooler<MediumReapSkill> reapPool;
    [SerializeField] private OnlinePooler<MediumSoulStrikeSkill> soulStrikPool;
    
    [SerializeField] private GameObject reapPrefab;
    
    private MediumReapSkill reap;
    private bool reapIsCasting = false;
    
    public override void InitializeClass()
    {
        GivePoolOwnership(base.Owner);

        SpawnReap(base.Owner);
        soulStrikPool.InitializePool();
    }

    [ServerRpc]
    private void GivePoolOwnership(NetworkConnection nc)
    {
        soulStrikPool.GiveOwnership(base.Owner);
    }

    public override void DeinitializeClass()
    {
        soulStrikPool.DeinitializePool();
        reap.Deinitialize();
    }


    #region ReapSkill

    protected override void CastSkill1(bool state)
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

    private async UniTask StartReap()
    {
        //reap = await reapPool.Get();
        reap.OnSpawned();
        reap.SkillFinished += ReturnReap;
        reap.transform.parent = this.transform;
        reap.transform.localPosition = Vector3.zero;
        reap.StartReap(10);//TODO: add to stat system 
        StartReapServer(reap.transform);
    }

    [ServerRpc]
    private void StartReapServer(Transform reap)
    {
        StartReapClients(reap);
    }

    [ObserversRpc]
    private void StartReapClients(Transform reap)
    {
        if (base.IsOwner)
        {
            return;
        }
        
        reap.parent = this.transform;
        reap.localPosition = Vector3.zero;
    }

    private void StopReap()
    {
        reap.StopReap();
    }
    
    private void ReturnReap(Skill skill)
    {
        reap.OnReturned();
        //reapPool.Return(reap);
    }
    
    #endregion


    protected override void CastSkill2(bool state)
    {
        if (state)
        {
            
        }
        else
        {
            CastSoulStrike();
        }
    }

    private async UniTask CastSoulStrike()
    {
        var soulStrike = soulStrikPool.Get();
        soulStrike.SkillFinished += ReturnSoulStrike;
        
        soulStrike.transform.position = this.transform.position;

        soulStrike.CastSoulStrikeOwner(lookDir);
        
        CastSoulStrikeServer(soulStrike.transform,this.transform.position);
    }
    
    [ServerRpc]
    private void CastSoulStrikeServer(Transform soulStrike,Vector3 position)
    {
        CastSoulStrikeClients(soulStrike,position);
    }

    [ObserversRpc]
    private void CastSoulStrikeClients(Transform soulStrike,Vector3 position)
    {
        if (base.IsOwner)
        {
            return;
        }
        soulStrike.transform.position = this.transform.position;
    }

    private void ReturnSoulStrike(Skill skill)
    {
        soulStrikPool.Return((MediumSoulStrikeSkill)skill);
    }

    [ServerRpc]
    private void SpawnReap(NetworkConnection nc)
    {
        GameObject o = Instantiate(reapPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
        ServerManager.Spawn(o,nc);
        
        var reapSkill = o.GetComponent<MediumReapSkill>();
        reapSkill.Initialize(nc);
        GetReapReference(nc, reapSkill);
    }

    [TargetRpc]
    private void GetReapReference(NetworkConnection nc,MediumReapSkill skillReference)
    {
        reap = skillReference;
        reap.GiveOwnership(nc);
        reap.OnGenerated();
    }
}
