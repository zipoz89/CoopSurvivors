using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class MediumReapSouls : Skill
{
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float moveTime = 1 ; //TODO: add to stat system 

    private Coroutine MoveToPlayerCoroutine;
    private Vector3 startPos;
    private Transform playerTransform;
    
    public void StartReap(Vector3 startPos,Transform playerTransform)
    {
        //Debug.Log("Start reap soul",this);
        
        this.startPos = startPos;
        this.playerTransform = playerTransform;
        StartReapServer(startPos, playerTransform);
        
        MoveToPlayerCoroutine = StartCoroutine(ReapSoulMoveToPlayer());
    }

    [ServerRpc]
    private void StartReapServer(Vector3 startPos, Transform playerTransform)
    {
        StartReapClient(startPos, playerTransform);
    }

    [ObserversRpc]
    private void StartReapClient(Vector3 startPos, Transform playerTransform)
    {
        if (base.IsOwner)
        {
            return;
        }
        this.playerTransform = playerTransform;
        this.startPos = startPos;
        MoveToPlayerCoroutine = StartCoroutine(ReapSoulMoveToPlayer());
    }
    
    private IEnumerator ReapSoulMoveToPlayer()
    {
        float elapsedTime = 0;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / moveTime;
            
            transform.transform.position = Vector3.Lerp(startPos,playerTransform.position,moveCurve.Evaluate(progress));
            
            yield return null;
        }

        Finish();
    }

    public void Finish()
    {
        FinishReapServer();
        StopCoroutine(MoveToPlayerCoroutine);
        SkillFinished?.Invoke(this);
    }

    [ServerRpc]
    private void FinishReapServer()
    {
        FinishReapClient();
    }

    [ObserversRpc]
    private void FinishReapClient()
    {
        if (base.IsOwner)
        {
            return;
        }
        
        StopCoroutine(MoveToPlayerCoroutine);
    }
    
}
