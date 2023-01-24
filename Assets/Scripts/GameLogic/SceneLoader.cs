using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneManager _sceneManager;
    public SceneManager SceneManager => _sceneManager;
    
    public enum Scene
    {
        None,
        CthuluLobby,
        CthuluMap1,
    }

    public Dictionary<Scene, string> scenes = new ();

    public HashSet<string> mapScenes = new ();

    private Scene lastScene;
    private Scene currentScene;
    private Scene nextScene;

    public void SetNextScene(Scene next)
    {
        nextScene = next;
    }

    private void Awake()
    {
        scenes.Add(Scene.CthuluMap1,"CthuluMap1");
        scenes.Add(Scene.CthuluLobby,"CthuluLobby");

        lastScene = Scene.None;
        currentScene = Scene.CthuluLobby;
        nextScene = Scene.CthuluMap1;

        mapScenes.Add(scenes[Scene.CthuluMap1]);
        
        _sceneManager.OnClientLoadedStartScenes += LoadLobbyScene;
    }

    private void LoadLobbyScene(NetworkConnection conn, bool asServer)
    {
        if (asServer)
        {
            SceneLoadData sld = new SceneLoadData(scenes[Scene.CthuluLobby]);
            _sceneManager.LoadGlobalScenes(sld);
        }
    }

    public void StartMap()
    {
        Debug.Log("Start Map");

        if (nextScene == Scene.None)
        {
            Debug.LogError("Next scene is set to None!");
            return;
        }

        SceneLoadData sld = new SceneLoadData(scenes[nextScene]);
        sld.MovedNetworkObjects = GameManager.Instance.PlayerManager.GetPlayersNetworkObjects();

        lastScene = currentScene;
        currentScene = nextScene;
        nextScene = Scene.None;
        
        _sceneManager.LoadGlobalScenes(sld);
        
        SceneUnloadData sud = new SceneUnloadData(scenes[lastScene]);
        _sceneManager.UnloadGlobalScenes(sud);
    }
    
    
}
