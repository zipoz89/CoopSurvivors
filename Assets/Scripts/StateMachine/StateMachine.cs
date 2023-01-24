using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State currentState;

    
    private LobbyState lobbyState;

    private Dictionary<States, State> stateDictionary = new Dictionary<States, State>();

    
    
    public enum States
    {
        LobbyState
    }

    public void Initialize()
    {
        lobbyState = new LobbyState();
        
        stateDictionary.Add(States.LobbyState,lobbyState);
    }

    public void UpdateState()
    {
        currentState.OnStateUpdate();
    }

    public void ChangeState(States state)
    {
        currentState?.OnStateExit();
        var ts = stateDictionary[state];
        ts.OnStateEnter();
        currentState = ts;
    }

}
