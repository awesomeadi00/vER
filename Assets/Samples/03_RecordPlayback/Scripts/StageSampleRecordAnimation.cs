using UnityEngine;
using UnityEngine.Networking;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace QuickVR.Samples.RecordAnimation
{

    public class StageSampleRecordAnimation : QuickStageBase
    {

        #region PUBLIC ATTRIBUTES

        public SampleRecordAnimationUI _gui = null;

        #endregion

        #region PROTECTED ATTRIBUTES

        protected bool _isRecording = false;

        public QuickAnimationPlayer _animationPlayerSrc
        {
            get
            {
                return QuickSingletonManager.GetInstance<QuickVRManager>().GetAnimatorSource().GetOrCreateComponent<QuickAnimationPlayer>();
            }

        }

        #endregion

        #region CREATION AND DESTRUCTION

        protected override void Start()
        {
            _maxTimeOut = -1;   //Just to ensure that this stage is executed forever. 
            ShowGUI(false);

            base.Start();
        }

        #endregion

        #region GET AND SET

        protected virtual void ShowGUI(bool show)
        {
            _gui.gameObject.SetActive(show);
            _interactionManager._interactorHandRight.SetInteractorEnabled(InteractorType.UI, show);
        }

        #endregion

        #region UPDATE

        protected override void Update()
        {
            if (InputManager.GetButtonDown("ShowGUI"))
            {
                ShowGUI(!_gui.gameObject.activeSelf);
            }

            if (InputManagerKeyboard.GetKeyDown(UnityEngine.InputSystem.Key.J))
            {
                if (!_animationPlayerSrc.IsRecording())
                {
                    Debug.Log("START RECORDING!!!");
                    _animationPlayerSrc.Record();
                }
                else
                {
                    Debug.Log("STOP RECORDING!!!");
                    _animationPlayerSrc.StopRecording();
                    QuickAnimationUtils.SaveToAnim("test.anim", _animationPlayerSrc.GetRecordedAnimation());
                    QuickAnimationUtils.SaveToJson("test.json", _animationPlayerSrc.GetRecordedAnimation());
                }
            }

            base.Update();
        }

        #endregion

    }

}


