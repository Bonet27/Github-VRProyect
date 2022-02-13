using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;

    public void ChangeState(State _newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = _newState;
        currentState.Enter();
    }

    public void SMUpdate()
    {
        currentState?.Update();
    }
}
