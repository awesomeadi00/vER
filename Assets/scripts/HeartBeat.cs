using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour
{
    private AudioSource audioSource; // The audio source component
    public float increaseDuration = 5f; // Duration to reach max volume
    public float holdDuration = 2f; // Duration to hold at max volume
    public float decreaseDuration = 5f; // Duration to drop to end volume
    public float startVolume = 0f; // The volume to start at
    public float endVolume = 0f; // The volume to end at
    public float maxVolume = 1f; // The maximum volume to reach


    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject!");
            return;
        }

        StartCoroutine(AdjustVolume());
    }

    IEnumerator AdjustVolume()
    {
        // Set the starting volume and ensure audio is playing
        audioSource.volume = startVolume;
        audioSource.Play();

        // Gradually increase volume to maxVolume
        float elapsedTime = 0f;
        while (elapsedTime < increaseDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, maxVolume, elapsedTime / increaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = maxVolume;

        // Hold at max volume
        yield return new WaitForSeconds(holdDuration);

        // Gradually decrease volume to endVolume
        elapsedTime = 0f;
        while (elapsedTime < decreaseDuration)
        {
            audioSource.volume = Mathf.Lerp(maxVolume, endVolume, elapsedTime / decreaseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = endVolume;

        // Stop the audio after reaching end volume, if it's zero
        if (Mathf.Approximately(endVolume, 0f))
        {
            audioSource.Stop();
        }
    }
}
