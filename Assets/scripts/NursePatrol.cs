using System.Collections;
using UnityEngine;

public class NursePatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public float movementSpeed = 2f; // Speed of movement between patrol points
    public float idleTimeMin = 2f; // Minimum idle time at a patrol point
    public float idleTimeMax = 5f; // Maximum idle time at a patrol point

    private Animator animator;
    private int currentPatrolIndex = 0; // Index of the current patrol point
    private bool movingForward = true; // Determines direction of patrol
    private bool isWalking = false; // Animation state: true = walking, false = idle

    void Start()
    {
        animator = GetComponent<Animator>();

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned!");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject!");
            return;
        }

        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Start walking
            SetWalking(true);

            // Move to the current patrol point
            yield return MoveToPoint(patrolPoints[currentPatrolIndex].position);
            Debug.Log(currentPatrolIndex);

            // Reached patrol point, idle for a random time
            SetWalking(false);
            yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));

            // Choose the next patrol point
            if (movingForward)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = 0;
                }
            }
        }
    }

    IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            
            // Face the direction of movement
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction.magnitude > 0.1f)
                transform.forward = direction;

            yield return null;
        }
    }

    void SetWalking(bool walking)
    {
        isWalking = walking;
        animator.SetBool("isWalking", isWalking); // Update animator state
    }
}
