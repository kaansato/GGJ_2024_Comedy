using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;

namespace GGJFUK
{
    public enum ComedianState
    {
        Idle,
        Walk,
        Talk,
        Win,
        Rare,
        Fall,
        Length
    }

    public class Comedian : MonoBehaviour
    {
        public Animator animator;

        public ComedianState comedianState = ComedianState.Idle;

        Move move = null;

        void Start()
        {
            StartWalking();
        }

        void StartWalking()
        {
            comedianState = ComedianState.Walk;
            SetAnimation();

            Vector3 startPosition = new Vector3(0.1f, -17.6f, -6.5f);
            Vector3 endPosition = new Vector3(0.1f, -17.6f, 0.0f);

            move = new Move(this.gameObject, startPosition, endPosition, 0, 3f,
                    tweenType: TransitionHelper.TweenType.linear, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            move.Start();
        }

        void LogStart()
        {
            Debug.Log("StartWalking Start");
        }

        void LogUpdate(float progress)
        {
            //Debug.Log("StartWalking Update:" + progress);
        }

        void LogComplete()
        {
            Debug.Log("StartWalking Complete: " + comedianState);

            move = null;

            if (comedianState == ComedianState.Walk)
            {
                comedianState = ComedianState.Idle;
                SetAnimation();

                GameManager.Instance.audioManager.StopAll();

                GameManager.Instance.audienceManager.SetAudienceState(AudienceState.Idle);

                Invoke("StartTalking", 0.5f);
            }
            else
            {
                GameManager.Instance.EndStage();

                Destroy(this.gameObject);
            }
        }

        void StartTalking()
        {
            GameManager.Instance.rhythmGame.StartGame();

            GameManager.Instance.uIManager.laughSlider.gameObject.SetActive(true);

            comedianState = ComedianState.Talk;
            SetAnimation();

            GameManager.Instance.audioManager.PlayLoopAudio(GameManager.Instance.audioManager.talkingAudio, 1f);

            //Invoke("StopTalking", 10.0f);
        }

        public void StartWin()
        {
            comedianState = ComedianState.Win;
            SetAnimation();

            Invoke("StopWin", 3.0f);
        }

        void StopWin()
        {
            comedianState = ComedianState.Rare;
            SetAnimation();

            GameManager.Instance.audioManager.StopAll();

            GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.dinkAudio);

            GameManager.Instance.OpenGates();

            //StartLaugh();

            Invoke("StartFalling", 1.0f);
        }

        void StartFalling()
        {
            comedianState = ComedianState.Fall;
            SetAnimation();

            Vector3 startPosition = new Vector3(0.1f, -17.6f, 0.0f);
            Vector3 endPosition = new Vector3(0.1f, -22.6f, 0.0f);

            move = new Move(this.gameObject, startPosition, endPosition, 0.2f, 0.8f,
                    tweenType: TransitionHelper.TweenType.linear, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            move.Start();

            GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.fallAudio);
        }

        void SetAnimation()
        {
            animator.SetInteger("State", (int)comedianState);
        }

        void Update()
        {

        }
    }
}
