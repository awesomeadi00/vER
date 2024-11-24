using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoctorMovement : MonoBehaviour
{
    public GameObject wheelChair;
    public Button readyButton;
    private AudioSource doctorAudio;
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
        doctorAudio = GetComponent<AudioSource>();

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

        wheelChair.SetActive(false);
        readyButton.gameObject.SetActive(false);

        StartCoroutine(DoctorBehaviorRoutine());
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

    IEnumerator DoctorBehaviorRoutine()
    {        
        // Initial idle phase
        yield return IdlePhase();

        // Walking phase (moving through patrol points)
        yield return WalkingToPatientPhase();

        // Talking phase
        yield return TalkingPhase();

        wheelChair.SetActive(true);
        readyButton.gameObject.SetActive(true);


        // Walking away phase (in reverse)
        yield return WalkingAwayPatientPhase();

        // Make the doctor disappear
        gameObject.SetActive(false);
    }

    IEnumerator IdlePhase()
    {
        Debug.Log("Doctor is idle.");
        yield return new WaitForSeconds(idleTime);
    }

    IEnumerator WalkingToPatientPhase()
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

        yield return StartCoroutine(SmoothRotate(transform.eulerAngles.y - 45f, 1f)); 

        // Trigger the 'near_user' animation parameter to transition to talking
        animator.SetBool("talking", true);

        doctorAudio.Play();

        // Wait for the talking animation duration
        yield return new WaitForSeconds(talkingTime);

        // Wait for the talking animation to finish
        animator.SetBool("talking", false);

        // Wait until the animation transitions out of "Talking"
        yield return new WaitUntil(() => IsAnimationDone("Talking"));

        doctorAudio.Stop();
        
        Debug.Log("Doctor finished talking.");
    }

    IEnumerator WalkingAwayPatientPhase()
    {
        Debug.Log("Doctor walks away.");

        yield return StartCoroutine(SmoothRotate(transform.eulerAngles.y + 225f, 1f)); 

        // Trigger the 'enter' animation parameter to transition to walking
        animator.SetTrigger("enter");
        isMoving = true;

        // Walk back through the patrol points in reverse order
        while (currentPatrolIndex > 0)
        {
            currentPatrolIndex--; // Move to the previous patrol point
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
        }

        isMoving = false;
    }

    IEnumerator SmoothRotate(float targetAngleY, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngleY, 0);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure it snaps to the exact target angle at the end
    }


        bool IsAnimationDone(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName(animationName);
    }
}
