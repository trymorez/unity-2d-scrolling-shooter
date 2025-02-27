using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] Vector2[] waypointsPos;
    public int currentPoint;
    public bool isLastWaypoint => currentPoint == waypointsPos.Length - 1;

    void Awake()
    {
        var currentPos = (Vector2) transform.position;
        waypointsPos = new Vector2[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointsPos[i] = (Vector2)waypoints[i].position - currentPos;
            currentPos = waypoints[i].position;
        }
    }

    public void NextWaypoint()
    {
        if (currentPoint < waypointsPos.Length - 1)
        {
            currentPoint++;
        }
    }

    public Vector3 GetWaypoint()
    {
        return waypointsPos[currentPoint];
    }

    void OnDrawGizmosSelected()
    {
        if (waypointsPos.Length > 0)
        {
            Gizmos.color = Color.red;

            Vector2 startPoint = (Vector2) transform.position;
            Vector2 fromPoint = startPoint;

            for (int i = 0; i < waypointsPos.Length; i++)
            {
                Gizmos.DrawLine(fromPoint, fromPoint + waypointsPos[i]);
                fromPoint += waypointsPos[i];
            }
        }
    }
}
