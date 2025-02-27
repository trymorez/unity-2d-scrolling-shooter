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

    bool isWaypointEnd;

    public override void UpdateState()
    {
        if (!isWaypointEnd)
        {
            transform.Translate(Vector3.up * SmallTank.MoveSpeed * Time.deltaTime);
            Vector2 delta = transform.localPosition - nextPos;
            float distance = delta.magnitude;
            //Debug.Log(distance);
            if (distance <= minDistance)
            {
                //Debug.Log("Next!");
                transform.localPosition = nextPos;
                SmallTank.previousPos = nextPos;
                if (waypoints.isLastWaypoint)
                {
                    //Debug.Log("end!");
                    isWaypointEnd = true;
                }
                waypoints.NextPoint();
                Vector3 vectorToNextWaypoint = waypoints.GetWaypoint();
                nextPos = SmallTank.previousPos + vectorToNextWaypoint;

                float angleZ = Mathf.Atan2(vectorToNextWaypoint.y, vectorToNextWaypoint.x) * Mathf.Rad2Deg +270f;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleZ));
            }
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
