
using UnityEngine;

public class RoundState : State
{
    private int round = 0;
    
    public override void OnStateEnter()
    {
        round++;
        Debug.Log(round +" Round started");
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}
