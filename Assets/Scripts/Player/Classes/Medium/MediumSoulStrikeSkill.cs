using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumSoulStrikeSkill : Skill
{
    [SerializeField] private float baseSpeed = 1f;//TODO: add to stat system 
    [SerializeField] private float baseLifeTime = 3f;//TODO: add to stat system 
    [SerializeField] private TrailRenderer trail;
    
    private Coroutine flyCoroutine;

    private Vector2 flyDir; 
    
    public void CastSoulStrikeOwner(Vector2 dir)
    {
        SkillResetCooldown?.Invoke(true);
        CastSoulStrike(dir);
        StartSoulServer(dir);
    }
    
    [ServerRpc]
    private void StartSoulServer(Vector2 dir)
    {
        StartSoulClient(dir);
    }

    [ObserversRpc]
    private void StartSoulClient(Vector2 dir)
    {
        if (base.IsOwner)
        {
            return;
        }

        CastSoulStrike(dir);
    }

    private void CastSoulStrike(Vector2 dir)
    {
        flyDir = dir;
        flyCoroutine = StartCoroutine(ReapExpand());
    }

    private IEnumerator ReapExpand()
    {
        float elapsedTime = 0;

        while (elapsedTime < baseLifeTime)
        {
            elapsedTime += Time.deltaTime;
            
            var posToAdd = (Vector3)flyDir * (baseSpeed * Time.deltaTime);

            this.transform.position += posToAdd;
            
            yield return null;
        }

        StopSoulStrikeOwner();
    }
    
    private void StopSoulStrikeOwner()
    {
        StopSoulStrike();
        StopSoulStrikeServer();

        
        SkillFinished?.Invoke(this);
    }

    private void StopSoulStrike()
    {
        StopCoroutine(flyCoroutine);
    }

    [ServerRpc]
    private void StopSoulStrikeServer()
    {
        StopSoulStrikeClient();
    }

    [ObserversRpc]
    private void StopSoulStrikeClient()
    {
        if (base.IsOwner)
        {
            return;
        }

        StopSoulStrike();
    }
    
    public override void OnReturned()
    {
        base.OnReturned();
        trail.Clear();
    }
    

    [ObserversRpc]
    protected override void OnReturnedClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        graphicObject.SetActive(false);
        trail.Clear();
    }
}
