using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;

namespace GGJFUK
{
    public class Gate : MonoBehaviour
    {
        Vector3 firstRotation;

        Rotate rotate = null;

        void Start()
        {
            firstRotation = transform.localEulerAngles;

            //Invoke("OpenGate", 2.0f);
        }

        public void OpenGate()
        {
            Vector3 startRotation = firstRotation;
            Vector3 endRotation = new Vector3(firstRotation.x, firstRotation.y, 60.0f);
            firstRotation = endRotation;

            rotate = new Rotate(gameObject, startRotation, endRotation, 0, 0.5f,
                    tweenType: TransitionHelper.TweenType.linear, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            rotate.Start();
        }

        public void CloseGate()
        {
            Vector3 startRotation = firstRotation;
            Vector3 endRotation = new Vector3(firstRotation.x, firstRotation.y, 0.0f);

            rotate = new Rotate(gameObject, startRotation, endRotation, 0, 0.5f,
                    tweenType: TransitionHelper.TweenType.linear, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            rotate.Start();
        }

        void LogStart()
        {
            Debug.Log("Gate Start");
        }

        void LogUpdate(float progress)
        {
        }

        void LogComplete()
        {
            Debug.Log("Gate Complete");

            rotate = null;
        }
    }
}
