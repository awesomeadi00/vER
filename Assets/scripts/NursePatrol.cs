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
    private bool isIdleLooking = false; // Tracks if the nurse is looking around during idle

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

    void LockRotation()
    {
        // Keep the rotation on x and z axes fixed, and allow only y-axis rotation
        Vector3 fixedRotation = transform.eulerAngles;
        fixedRotation.x = 0f; // Lock x rotation
        fixedRotation.z = 0f; // Lock z rotation
        transform.eulerAngles = fixedRotation;
    }

    void Update()
    {
        LockRotation();
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Start walking
            SetWalking(true);

            // Move to the current patrol point
            yield return MoveToPoint(patrolPoints[currentPatrolIndex].position);

            // Reached patrol point, idle for a random time
            SetWalking(false);

            // Look around randomly while idle
            yield return IdleLookAround();

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

    IEnumerator IdleLookAround()
    {
        isIdleLooking = true;
        SetLooking(true); // Trigger the looking animation
        float idleDuration = Random.Range(idleTimeMin, idleTimeMax); // How long to remain idle
        float timer = 0f;

        while (timer < idleDuration)
        {
            float lookAngle = Random.Range(-30f, 30f); // Random angle to look left or right
            float lookSpeed = Random.Range(50f, 200f); // Random speed for the look movement

            Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + lookAngle, 0);

            // Smoothly rotate to the target angle
            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
                yield return null;
            }

            // Pause for a moment after looking
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            timer += Random.Range(1f, 2f); // Increment the timer randomly
        }

        // Stop the looking animation
        SetLooking(false);

        // Wait until the animation transitions out of "Looking"
        yield return new WaitUntil(() => IsAnimationDone("Looking"));
    }

    bool IsAnimationDone(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName(animationName);
    }

    void SetWalking(bool walking)
    {
        isWalking = walking;
        animator.SetBool("isWalking", isWalking); // Update animator state
    }

    void SetLooking(bool looking)
    {
        animator.SetBool("looking", looking); // Update looking animation state
    }
}
