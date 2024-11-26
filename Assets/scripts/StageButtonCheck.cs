using UnityEngine;
using System.Collections;
using TMPro; // Import TextMeshPro namespace


namespace QuickVR
{
    public class StageButtonCheck : QuickStageBase
    {
        #region PUBLIC PARAMETERS

        [Header("Custom Settings")]
        public GameObject targetObject; // The object that must be active for _pressKeyToFinish to be true.
        public TextMeshProUGUI targetText; // The TextMeshProUGUI component to update.


        #endregion

        #region OVERRIDE METHODS

        protected override void Update()
        {
            base.Update();

            // Check if the target object is active in the scene.
            if (targetObject != null)
            {
                _pressKeyToFinish = targetObject.activeInHierarchy;
            }

            // Check for VR button press and call Finish()
            if (_pressKeyToFinish && IsRightTriggerPressed())
            {
                UpdateText("Good Luck!");
                Finish();
            }
        }

        public IEnumerator WaitForButtonPress()
        {
            // Wait until _pressKeyToFinish is true and the button is pressed.
            while (!_pressKeyToFinish || !IsRightTriggerPressed())
            {
                yield return null;
            }
        }

        protected override IEnumerator CoUpdate()
        {
            // Perform any initial setup or checks.
            yield return base.CoUpdate();

            // Wait for the button press.
            yield return StartCoroutine(WaitForButtonPress());

            // Proceed with the next steps after the button press.
            Debug.Log("Button was pressed. Proceeding...");
        }

        #endregion

        #region PRIVATE METHODS

        private bool IsRightTriggerPressed()
        {
            // Check if the right trigger button is pressed (default Unity Input Manager mapping)
            return Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.1f;
        }

        private void UpdateText(string newText)
        {
            // Check if the TextMeshProUGUI component is assigned and update its text.
            if (targetText != null)
            {
                targetText.text = newText;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component is not assigned.");
            }
        }

        #endregion
    }
}
