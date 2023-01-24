using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PlayerManager playerManager;
    public PlayerManager PlayerManager => playerManager;
    
    
    [SerializeField] private PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;
    
    [SerializeField] private SceneLoader sceneLoader;
    public SceneLoader SceneLoader => sceneLoader;

    private MapManager mapManager;
    public MapManager MapManager => mapManager;

    protected override void Awake()
    {
        base.Awake();
        sceneLoader.SceneManager.OnQueueEnd += NewSceneLoaded;
    }


    private void NewSceneLoaded()
    {
        
        bool mapManagerFound = TryGetMapManager();
        
        
        if (mapManagerFound)
        {
            SetMapManagerClient(mapManager);
            mapManager.StartGame();
        }
    }

    [ObserversRpc]
    private void SetMapManagerClient(MapManager mapManager)
    {
        if (base.IsServer)
        {
            return;
        }

        this.mapManager = mapManager;
    }

    private bool TryGetMapManager()
    {
        mapManager = FindObjectOfType<MapManager>();

        return mapManager != null;
    }
}
