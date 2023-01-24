using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Unity.Mathematics;
using UnityEngine;

public class StartMapButton : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer loadingRenderer;
    [SerializeField] private List<Player> players = new List<Player>();
    [SerializeField] private float loadSpeed = 1f;
    
    [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(on_procentage))]
    private float LoadProcentage = 0;


    private float targetProgress = 0;
    
    private bool gameStarted = false;
    
    private void Update()
    {
        UpdateLoadingBar();
        
        if (IsServer)
        {
            if (gameStarted)
            {
                return;
            }

            if (players.Count > 0)
            {
                LoadProcentage += Time.deltaTime * loadSpeed * players.Count/GameManager.Instance.PlayerManager.PlayerCount;

                if (LoadProcentage > 1 && gameStarted == false)
                {
                    LoadProcentage = 1;
                    gameStarted = true;
                    StartGameServer();
                }
            }
            else if (LoadProcentage > 0)
            {
                LoadProcentage -= Time.deltaTime * 0.2f;

                if (LoadProcentage < 0)
                {
                    LoadProcentage = 0;
                }
            }
        }
    }

    private void UpdateLoadingBar()
    {
        var currentValue = loadingRenderer.sharedMaterial.GetFloat("_Progress");
        var valueToDisplay = currentValue < 0.95f ? math.lerp(currentValue, targetProgress, 0.5f) : 1;
        loadingRenderer.sharedMaterial.SetFloat("_Progress",valueToDisplay);

    }


    [Server]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                players.Add(other.GetComponent<Player>());
            }
        }
    }

    [Server]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                players.Remove(other.GetComponent<Player>());
            }
        }
    }
    
    private void on_procentage(float prev, float next, bool asServer)
    {
        targetProgress = next;
    }
    
    [ObserversRpc]
    public void StartGameClient()
    {
        GameManager.Instance.SceneLoader.StartMap();
    }
    
    public void StartGameServer()
    {
        GameManager.Instance.StartGame();
    }

    private void OnDisable()
    {
        loadingRenderer.sharedMaterial.SetFloat("_Progress", 0);
    }
}
