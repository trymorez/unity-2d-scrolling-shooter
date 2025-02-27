using UnityEngine;
using static Unity.VisualScripting.Member;

public class MoveState : BaseState<STankState>
{
    public SmallTank SmallTank;
    Waypoints waypoints;
    float ellapsedMoveTime;
    Vector3 previousPos;
    Vector3 nextPos;
    Transform transform;
    float minDistance = 0.02f;

    public MoveState(SmallTank smallTank) : base(STankState.Move) 
    { 
        SmallTank = smallTank; 
    }

    public override void EnterState()
    {
        ellapsedMoveTime = 0;
        previousPos = SmallTank.previousPos;
        waypoints = SmallTank.Waypoints;
        transform = SmallTank.transform;

        Vector3 vectorToNextWaypoint = waypoints.GetWaypoint();
        nextPos = previousPos + vectorToNextWaypoint;
    }

    bool isWaypointEnded;

    public override void UpdateState()
    {
        if (SmallTank.IsTurning)
        {
            SmallTank.TurnToNextWaypont();
            return;
        }
        if (isWaypointEnded)
        {
            return;
        }

        transform.Translate(Vector3.up * SmallTank.MoveSpeed * Time.deltaTime);

        Vector2 delta = transform.localPosition - nextPos;
        float distance = delta.magnitude;

        //waypoint reached
        if (distance <= minDistance)
        {
            transform.localPosition = nextPos;
            SmallTank.previousPos = nextPos;
            if (waypoints.isLastWaypoint)
            {
                isWaypointEnded = true;
            }
            waypoints.NextWaypoint();
            Vector3 vectorToNextWaypoint = waypoints.GetWaypoint();
            nextPos = SmallTank.previousPos + vectorToNextWaypoint;
            SmallTank.IsTurning = true;
        }
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
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
    }

    public override void OnTriggerStay2D(Collider2D other)
    {
    }
}
