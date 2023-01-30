using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class MediumClass : PlayerClass
{
    [SerializeField] private OnlinePooler<MediumReapSkill> reapPool;
    [SerializeField] private OnlinePooler<MediumSoulStrikeSkill> soulStrikPool;

    public override void InitializeClass()
    {
        GivePoolOwnership(base.Owner);
        
        reapPool.InitializePool();
        soulStrikPool.InitializePool();
    }

    [ServerRpc]
    private void GivePoolOwnership(NetworkConnection nc)
    {
        reapPool.GiveOwnership(base.Owner);
        soulStrikPool.GiveOwnership(base.Owner);
    }

    public override void DeinitializeClass()
    {
        reapPool.DeinitializePool();
        soulStrikPool.DeinitializePool();
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
    
    private async UniTask StartReap()
    {
        reap = await reapPool.Get();
        reap.SkillFinished += ReturnReap;
        reap.transform.parent = this.transform;
        reap.transform.localPosition = Vector3.zero;
        reap.StartReap(4);//TODO: add to stat system 
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
