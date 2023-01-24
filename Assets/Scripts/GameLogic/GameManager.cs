using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private StateMachine stateMachine;
    public StateMachine StateMachine => stateMachine;
    

    protected override void Awake()
    {
        base.Awake();
        
        sceneLoader.SceneManager.OnLoadEnd += SceneLoaded;
    }


    private void SceneLoaded(SceneLoadEndEventArgs obj)
    {
        var scenes = obj.LoadedScenes;

        for (int i = 0; i < scenes.Length; i++)
        {
            if(scenes[i].name == SceneLoader.scenes[SceneLoader.Scene.CthuluLobby])
            {
                stateMachine.InitializeAndStartLobby();
            }

            if (SceneLoader.mapScenes.Contains(scenes[i].name))
            {
                SetPlayerLoadedServer();
                    
                bool mapManagerFound = TryGetMapManager();

                if (mapManagerFound)
                {
                    SetMapManagerClient(mapManager,scenes);
                }
            }
        }
    }

    public void StartGame()
    {
        stateMachine.ChangeState(StateMachine.States.StartingGameState);
        sceneLoader.StartMap();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerLoadedServer()
    {
        playerManager.SetPlayerLoadedOnMap();
    }


    [ObserversRpc(RunLocally = true)]
    private void SetMapManagerClient(MapManager mapManager,Scene[] scenes)
    {
        this.mapManager = mapManager;
    }

    private bool TryGetMapManager()
    {
        mapManager = FindObjectOfType<MapManager>();

        return mapManager != null;
    }
}
