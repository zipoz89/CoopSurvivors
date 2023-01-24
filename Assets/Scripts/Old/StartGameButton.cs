using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace Old
{

    public class StartGameButton : NetworkBehaviour
    {
        [SyncVar(Channel = Channel.Unreliable, OnChange = nameof(on_procentage))] [SerializeField]
        private float LoadProcentage = 0;

        [SerializeField] private SpriteRenderer loadingRenderer;

        [SerializeField] private GameObject parent;

        [SerializeField] private List<Player> players = new List<Player>();

        [SerializeField] private GameDirector gameDirector;

        private bool gameStarted = false;

        private void Update()
        {
            if (IsServer)
            {
                if (gameStarted)
                {
                    return;
                }

                if (players.Count > 0)
                {
                    LoadProcentage += Time.deltaTime * 1f;

                    if (LoadProcentage > 1 && gameStarted == false)
                    {
                        LoadProcentage = 1;
                        gameStarted = true;
                        StartGameClient();
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
            loadingRenderer.sharedMaterial.SetFloat("_Progress", next);
        }

        private void OnDisable()
        {
            loadingRenderer.sharedMaterial.SetFloat("_Progress", 0);
        }


        [ObserversRpc]
        public void StartGameClient()
        {
            Debug.Log("Start from button");
            parent.gameObject.SetActive(false);
            gameDirector.StartGame();
        }
    }
}