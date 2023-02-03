using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class MediumReapSkill : Skill
{
    [SerializeField] private OnlinePooler<MediumReapSouls> reapSoulsPool;
    
    [SerializeField] private float startRadius = 0.5f;
    private float maxRadius;

    [SerializeField] private AnimationCurve expandSpeed;
    [SerializeField] private float expandTime = 2 ; //TODO: add to stat system 
    [SerializeField] private float damage = 10; //TODO: add to stat system
    
    private Coroutine expandCoroutine;

    private List<IDamagable> enemiesInRadius = new List<IDamagable>();

    
    public void Initialize(NetworkConnection nc)
    {
        reapSoulsPool.GiveOwnership(nc);
        InitializeOnClient(nc);
    }

    [TargetRpc]
    private void InitializeOnClient(NetworkConnection nc)
    {
        reapSoulsPool.InitializePool();
    }

    public void Deinitialize()
    {
        reapSoulsPool.DeinitializePool();
    }

    public void StartReap(float maxDistance)
    {
        graphicObject.transform.localScale = new Vector3(startRadius, startRadius, 1);
        maxRadius = maxDistance;

        StartReapServer(maxDistance);
        
        expandCoroutine = StartCoroutine(ReapExpand());
    }

    [ServerRpc]
    private void StartReapServer(float maxDistance)
    {
        StartReapClient(maxDistance);
    }

    [ObserversRpc]
    private void StartReapClient(float maxDistance)
    {
        if (base.IsOwner)
        {
            return;
        }
        
        graphicObject.transform.localScale = new Vector3(startRadius, startRadius, 1);
        maxRadius = maxDistance;
        expandCoroutine = StartCoroutine(ReapExpand());
    }
    
    private IEnumerator ReapExpand()
    {
        float elapsedTime = 0;

        while (elapsedTime < expandTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / expandTime;
            
            transform.localScale = Vector3.Lerp(new Vector3(startRadius, startRadius, 1),new Vector3(maxRadius, maxRadius, 1),expandSpeed.Evaluate(progress));
            
            
            
            yield return null;
        }

        StopReap();
    }

    public async UniTask StopReap()
    {
        StopReapServer();
        StopCoroutine(expandCoroutine);

        foreach (var enemy in enemiesInRadius)
        {
            enemy.DealDamage(damage, DamageType.Magic,base.Owner);
            
            var reapSoul = reapSoulsPool.Get();
            
            reapSoul.SkillFinished += ReturnSoul;

            reapSoul.StartReap(enemy.GetGameObject().transform.position, this.transform); //TODO: transform tego musi być na graczu
        }
        
        enemiesInRadius.Clear();
        
        SkillResetCooldown?.Invoke(true);
        SkillFinished?.Invoke(this);
    }
    
    private void ReturnSoul(Skill skill)
    {
        skill.SkillFinished -= ReturnSoul;
        reapSoulsPool.Return((MediumReapSouls)skill);
    }

    [ServerRpc]
    private void StopReapServer()
    {
        StopReapClient();
    }

    [ObserversRpc]
    private void StopReapClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        
        StopCoroutine(expandCoroutine);
    }
    
    [Client]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("IDamagable"))
        {
            enemiesInRadius.Add(other.GetComponent<IDamagable>());
        }
    }
    
    [Client]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("IDamagable"))
        {
            //Debug.Log("removed to interactable list");
            enemiesInRadius.Remove(other.GetComponent<IDamagable>());
        }
    }
}
