using System.Collections;
using UnityEngine;

public class DoctorMovement : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    public float idleTime = 5f; // Time the doctor stays idle at the beginning
    public float talkingTime = 5f; // Time the doctor spends talking at the end
    public float movementSpeed = 2f; // Speed of movement between patrol points

    private Animator animator;
    private int currentPatrolIndex = 0; // Current patrol point index
    private bool isMoving = false; // To track if the doctor is currently moving

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

        StartCoroutine(DoctorBehaviorRoutine());
    }

    IEnumerator DoctorBehaviorRoutine()
    {
        // Initial idle phase
        yield return IdlePhase();

        // Walking phase (moving through patrol points)
        yield return WalkingPhase();

        // Talking phase
        yield return TalkingPhase();

        // Loop or extend behavior as needed here
    }

    IEnumerator IdlePhase()
    {
        Debug.Log("Doctor is idle.");
        yield return new WaitForSeconds(idleTime);
    }

    IEnumerator WalkingPhase()
    {
        Debug.Log("Doctor starts walking.");

        // Trigger the 'enter' animation parameter to transition to walking
        animator.SetTrigger("enter");
        isMoving = true;

        while (currentPatrolIndex < patrolPoints.Length)
        {
            Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;

            // Move towards the target patrol point
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

                // Face the direction of movement
                Vector3 direction = (targetPosition - transform.position).normalized;
                if (direction.magnitude > 0.1f)
                    transform.forward = direction;

                yield return null;
            }

            // Arrived at the patrol point, increment to the next point
            currentPatrolIndex++;
        }

        isMoving = false;
    }

    IEnumerator TalkingPhase()
    {
        Debug.Log("Doctor starts talking.");

        // Trigger the 'near_user' animation parameter to transition to talking
        animator.SetTrigger("near_user");

        // Wait for the talking animation duration
        yield return new WaitForSeconds(talkingTime);

        Debug.Log("Doctor finished talking.");
    }
}
