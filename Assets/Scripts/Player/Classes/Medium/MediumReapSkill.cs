using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumReapSkill : Skill
{
    [SerializeField] private GameObject colliderObject;

    [SerializeField] private float startRadius = 0.5f;
    private float maxRadius;

    [SerializeField] private AnimationCurve expandSpeed;
    [SerializeField] private float expandTime = 4 ; //TODO: add to stat system 

    private Coroutine expandCoroutine;
    
    public override void OnGenerated()
    {
        OnGeneratedClient();
    }

    [ObserversRpc]
    protected override void OnGeneratedClient()
    {
        colliderObject.SetActive(false);
    }

    public override void OnSpawned()
    {
        colliderObject.SetActive(true);
        OnSpawnedClient();
    }
    
    [ObserversRpc(RunLocally = false)]
    protected override void OnSpawnedClient()
    {
        colliderObject.SetActive(true);
    }

    public override void OnReturned()
    {
        colliderObject.SetActive(false);
        OnReturnedClient();
    }
    [ObserversRpc(RunLocally = false)]
    protected override void OnReturnedClient()
    {
        colliderObject.SetActive(false);
    }

    public void StartReap(float maxDistance)
    {
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

        ApplyReap();
    }

    public void ApplyReap()
    {
        StopCoroutine(expandCoroutine);
        SkillResetCooldown?.Invoke(true);
        SkillFinished?.Invoke();
    }

    
}
