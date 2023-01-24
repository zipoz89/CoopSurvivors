using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGameState : State
{
    private bool allPlayersConnected = false;
    private const float startGameTimerMax = 3;
    private float startGameTimerElapsed = 0;
    
    public override void OnStateEnter()
    {
        Debug.Log("StartingGameState entered");
    }

    public override void OnStateUpdate()
    {
        if (!allPlayersConnected)
        {
            WaitForAllPlayersLoaded();
        }
        else
        {
            WaitForGameStart();
            if (startGameTimerElapsed >= startGameTimerMax)
            {
                GameManager.Instance.StateMachine.ChangeState(StateMachine.States.RoundState);
            }
        }
    }

    public override void OnStateExit()
    {
        
    }

    private void WaitForAllPlayersLoaded()
    {
        if (GameManager.Instance.PlayerManager.PlayersLoadedOnMap == GameManager.Instance.PlayerManager.PlayerCount)
        {
            allPlayersConnected = true;
        }
    }

    private void WaitForGameStart()
    {
        startGameTimerElapsed += Time.deltaTime;
    }

}
