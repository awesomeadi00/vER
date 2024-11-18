using UnityEngine;

public class PatrolSquare : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f;
    public float waitTime = 1.0f;

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f; 

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop through waypoints
            }
        }
    }
}
