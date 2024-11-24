using System.Collections;
using UnityEngine;

public class FriendSupport : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component
    public float idleTime = 3f; // Time the friend stays idle before playing an emote
    public float doctorTime = 60f; // Time after which no more audio is allowed to play
    private bool isAnimating = false; // To track if the friend is performing an emote
    private bool canPlayAudio = true; // To track if audio is allowed to play

    private AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip[] audioClips; // Array to store 5 audio clips

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
            return;
        }

        if (audioClips == null || audioClips.Length < 5)
        {
            Debug.LogError("Please assign at least 5 audio clips!");
            return;
        }

        // Start the behavior loop
        StartCoroutine(FriendBehaviorLoop());

        // Start the timer to disable audio playback after doctorTime
        StartCoroutine(DisableAudioAfterTime(doctorTime));
    }

    void Update()
    {
        // Ensure the model's Y position stays constant at 0
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;
    }       

    IEnumerator FriendBehaviorLoop()
    {
        while (true)
        {
            // Idle phase
            yield return new WaitForSeconds(idleTime);

            // Emote phase
            int randomEmote = Random.Range(1, 6); // Select a random emote (1-5)

            if (randomEmote == 1)
            {
                animator.SetTrigger("cheering");
            }
            else if (randomEmote == 2)
            {
                animator.SetTrigger("fistpump");
            }
            else if (randomEmote == 3)
            {
                animator.SetTrigger("happy");
            }
            else if (randomEmote == 4)
            {
                animator.SetTrigger("clapping");
            }
            else if (randomEmote == 5)
            {
                animator.SetTrigger("thumbsup");
            }

            isAnimating = true;

            // Play a random audio clip if audio is allowed and no other model is speaking
            if (canPlayAudio && AudioSyncManager.Instance.CanSpeak())
            {
                PlayRandomAudioClip();
            }

            // Wait until the current animation finishes
            yield return new WaitUntil(() => IsAnimationDone());

            // Return to idle after emote
            isAnimating = false;
        }
    }

    private void PlayRandomAudioClip()
    {
        if (audioClips.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();

            // Notify the AudioSyncManager that this model is speaking
            AudioSyncManager.Instance.NotifySpeaking(audioSource.clip.length);
        }
    }

    private IEnumerator DisableAudioAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        canPlayAudio = false;
    }

    // Check if the current animation is done
    private bool IsAnimationDone()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the current animation is the idle state
        return stateInfo.IsName("Idle") || stateInfo.normalizedTime >= 1.0f;
    }
}
