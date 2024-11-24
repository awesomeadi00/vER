using UnityEngine;

namespace QuickVR
{
    public class StageButtonCheck : QuickStageBase
    {
        #region PUBLIC PARAMETERS

        [Header("Custom Settings")]
        public GameObject targetObject; // The object that must be active for _pressKeyToFinish to be true.

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
        }

        #endregion
    }
}
