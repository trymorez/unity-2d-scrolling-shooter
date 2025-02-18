using UnityEngine;

public class IdleState : BaseState<STankState>
{
    public SmallTank SmallTank;
    bool tartgetAcquired;

    public IdleState(SmallTank smallTank) : base(STankState.Idle) 
    { 
        SmallTank = smallTank;
    }

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
        if (other.CompareTag("Player"))
        {
            SmallTank.Target = other.transform;
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
