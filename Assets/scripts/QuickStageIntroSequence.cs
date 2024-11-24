using System.Collections;
using TMPro;
using UnityEngine;

namespace QuickVR
{
    public class QuickStageIntroText : QuickStagePreBase
    {
        #region PUBLIC ATTRIBUTES

        public string[] messages =
        {
            "Welcome to vER hospital.",
            "You are about to begin your surgery soon. Wait for your doctor. Good Luck."
        }; // Array of messages to display

        public float messageDisplayTime = 3.0f; // Duration to keep the message on screen
        public float fadeDuration = 0.5f; // Duration for fade-in and fade-out

        public Canvas introCanvas; // Reference to a Canvas
        public TextMeshProUGUI introText; // Reference to a Text component on the Canvas

        #endregion

        #region CREATION AND DESTRUCTION

        public override void Init()
        {
            base.Init();

            if (messages.Length == 0)
            {
                Debug.LogError("No messages defined for the intro sequence.");
            }

            if (!introCanvas || !introText)
            {
                Debug.LogError("Canvas or Text component is not assigned!");
            }

            // Ensure the Canvas is active and starts with invisible text
            introCanvas.gameObject.SetActive(true);
            introCanvas.GetComponent<CanvasGroup>().alpha = 0f; // Ensure transparency control
        }

        #endregion

        #region UPDATE

        protected override IEnumerator CoUpdate()
        {
            CanvasGroup canvasGroup = introCanvas.GetComponent<CanvasGroup>();

            // Display each message sequentially
            foreach (var message in messages)
            {
                yield return StartCoroutine(DisplayMessage(message, canvasGroup));
            }

            // Clear the text and hide the Canvas
            introText.text = ""; // Ensure text is cleared
            introCanvas.gameObject.SetActive(false);
        }

        #endregion

        #region CUSTOM METHODS

        private IEnumerator DisplayMessage(string message, CanvasGroup canvasGroup)
        {
            // Set the text
            introText.text = message;

            // Fade in
            yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f));

            // Wait while the message is fully visible
            yield return new WaitForSeconds(messageDisplayTime);

            // Fade out
            yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f));
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
        }

        #endregion
    }
}
