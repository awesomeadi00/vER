using System.Collections;
using UnityEngine;

public class PhoneRing : MonoBehaviour
{
    private AudioSource audioSource; 
    public float minInterval = 1f; // Minimum interval time in seconds
    public float maxInterval = 5f; // Maximum interval time in seconds
    public int playCount = 4; // Number of times to play the audio clip

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("No AudioSource or AudioClip found on this GameObject.");
            return;
        }

        // Start the coroutine to play the audio at random intervals
        StartCoroutine(PlayAudioAtRandomIntervals());
    }

    IEnumerator PlayAudioAtRandomIntervals()
    {
while (true) // Keep repeating the process indefinitely
        {
            // Wait for a random interval before starting the batch
            float randomWaitBeforeBatch = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomWaitBeforeBatch);

            // Play the audio clip playCount times consecutively
            for (int i = 0; i < playCount; i++)
            {
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }

            // Wait for another random interval after the batch
            float randomWaitAfterBatch = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomWaitAfterBatch);
        }
    }
}
