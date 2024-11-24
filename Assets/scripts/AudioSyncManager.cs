using UnityEngine;

public class AudioSyncManager : MonoBehaviour
{
    public static AudioSyncManager Instance; // Singleton instance

    private bool isSpeaking = false; // Tracks if any model is currently speaking
    private float speakingEndTime = 0f; // Tracks when the current speaking finishes

    private void Awake()
    {
        // Ensure a single instance of the manager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the manager across scenes if necessary
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Checks if a model can speak
    public bool CanSpeak()
    {
        return !isSpeaking;
    }

    // Notify the manager that a model has started speaking
    public void NotifySpeaking(float duration)
    {
        isSpeaking = true;
        speakingEndTime = Time.time + duration;
        StartCoroutine(ClearSpeakingFlag(duration));
    }

    // Coroutine to reset the speaking flag after the audio clip ends
    private System.Collections.IEnumerator ClearSpeakingFlag(float duration)
    {
        yield return new WaitForSeconds(duration);
        isSpeaking = false;
    }
}
