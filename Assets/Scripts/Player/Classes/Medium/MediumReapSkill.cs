using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumReapSkill : Skill
{
    

    [SerializeField] private float startRadius = 0.5f;
    private float maxRadius;

    [SerializeField] private AnimationCurve expandSpeed;
    [SerializeField] private float expandTime = 4 ; //TODO: add to stat system 

    private Coroutine expandCoroutine;
    
    public void StartReap(float maxDistance)
    {
        colliderObject.transform.localScale = new Vector3(startRadius, startRadius, 1);
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
        
        colliderObject.transform.localScale = new Vector3(startRadius, startRadius, 1);
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
            
            colliderObject.transform.localScale = Vector3.Lerp(new Vector3(startRadius, startRadius, 1),new Vector3(maxRadius, maxRadius, 1),expandSpeed.Evaluate(progress));
            
            yield return null;
        }

        StopReap();
    }

    public void StopReap()
    {
        StopReapServer();
        StopCoroutine(expandCoroutine);
        SkillResetCooldown?.Invoke(true);
        SkillFinished?.Invoke();
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
    
}
