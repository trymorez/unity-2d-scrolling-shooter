using UnityEngine;

public class IdleState : BaseState<STankState>
{
    public STank stank;
    bool tartgetAcquired;

    public IdleState() : base(STankState.Idle) { }

    public override void EnterState()
    {
        tartgetAcquired = false;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override STankState GetNextState()
    {
        if (!tartgetAcquired)
        {
            return STankState.Idle;
        }
        else
        {
            return STankState.Attack;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stank.target = other.transform;
            tartgetAcquired = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
