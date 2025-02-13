using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new();
    protected BaseState<EState> CurrentState;

    protected bool IsChangingState = false;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!IsChangingState && nextStateKey.Equals(CurrentState.StateKey))
        {
            Debug.Log("Updating State!");
            CurrentState.UpdateState();
        }
        else if (!IsChangingState)
        {
            Debug.Log("Changing State!");
            ChangeState(nextStateKey);
        }
    }

    public void ChangeState(EState stateKey)
    {
        IsChangingState = true;
        CurrentState?.ExitState();
        CurrentState = States[stateKey];
        CurrentState?.EnterState();
        IsChangingState = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter2D(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        CurrentState.OnTriggerStay2D(other);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit2D(other);
    }
}
