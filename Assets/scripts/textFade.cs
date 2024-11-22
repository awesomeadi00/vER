using UnityEngine;
using TMPro;
using System.Collections;

public class TextFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public float initialDelay = 2.0f;
    public float fadeInTime = 2.0f; 
    public float visibleTime = 10.0f;
    public float fadeOutTime = 2.0f;

    private TMP_Text textMesh;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        if(textMesh == null)
        {
            Debug.LogError("TextFader: No TextMeshPro component found on this GameObject.");
            return;
        }
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
        StartCoroutine(FadeTextRoutine());
    }

    IEnumerator FadeTextRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        float elapsedTime = 0;
        while(elapsedTime < fadeInTime)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, elapsedTime / fadeInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);

        yield return new WaitForSeconds(visibleTime);

        elapsedTime = 0;
        while(elapsedTime < fadeOutTime)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1 - (elapsedTime / fadeOutTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
    }
}
