using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;

    private Dictionary<States, State> stateDictionary = new Dictionary<States, State>();
    
    public enum States
    {
        LobbyState,
        StartingGameState,
        RoundState,
        InBetweenRoundState,
    }
    
    private LobbyState lobbyState;
    private StartingGameState startingGameState;
    private RoundState roundState;
    private InBetweenRoundState inBetweenRoundState;

    
    public void InitializeAndStartLobby()
    {
        lobbyState = new LobbyState();
        startingGameState = new StartingGameState();
        roundState = new RoundState();
        inBetweenRoundState = new InBetweenRoundState();

        stateDictionary.Add(States.LobbyState, lobbyState);
        stateDictionary.Add(States.StartingGameState, startingGameState);
        stateDictionary.Add(States.RoundState, roundState);
        stateDictionary.Add(States.InBetweenRoundState, inBetweenRoundState);

        ChangeState(States.LobbyState);
    }
    
    public void Update()
    {
        currentState?.OnStateUpdate();
    }
    
    public void ChangeState(States state)
    {
        currentState?.OnStateExit();
        var ts = stateDictionary[state];
        ts.OnStateEnter();
        currentState = ts;
    }
}
