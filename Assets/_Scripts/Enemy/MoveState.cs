using UnityEngine;

public class MoveState : BaseState<STankState>
{
    public SmallTank SmallTank;
    float ellapsedMoveTime;

    public MoveState() : base(STankState.Move) { }

    public override void EnterState()
    {
        ellapsedMoveTime = 0;
    }

    public override void UpdateState()
    {
        SmallTank.transform.Translate(Vector3.up * SmallTank.MoveSpeed * Time.deltaTime);
    }

    public override void ExitState()
    {
    }

    public override STankState GetNextState()
    {
        ellapsedMoveTime += Time.deltaTime;
        if (ellapsedMoveTime < SmallTank.DelayPerBurst)
        {
            return STankState.Move;
        }
        else
        {
            return STankState.Attack;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    SmallTank.Target = other.transform;
        //}
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
