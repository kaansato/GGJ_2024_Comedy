using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;

namespace GGJFUK
{
    public enum GenderType
    {
        Male,
        Female
    }

    public enum AudienceState
    {
        Idle,
        Clap,
        Laugh,
        Walk,
        Length
    }

    public class Audience : MonoBehaviour
    {
        public GenderType genderType;

        public AudienceState audienceState = AudienceState.Idle;

        Animator animator;

        Move move = null;

        Vector3 targetPosition;

        float walkSpeed = 3f;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimation()
        {
            animator.SetInteger("State", (int)audienceState);
        }

        public void WalkTo(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;

            audienceState = AudienceState.Walk;
            SetAnimation();

            Vector3 startPosition = transform.position;
            Vector3 endPosition = targetPosition;

            move = new Move(this.gameObject, startPosition, endPosition, 0, walkSpeed,
                    tweenType: TransitionHelper.TweenType.linear, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            move.Start();
        }

        void LogStart()
        {
            Debug.Log("Audience Start");
        }

        void LogUpdate(float progress)
        {
            //Debug.Log("Audience Update:" + progress);

            RotateToDir(targetPosition);
        }

        void LogComplete()
        {
            Debug.Log("Audience Complete: " + audienceState);

            audienceState = GameManager.Instance.audienceManager.audienceState;
            SetAnimation();

            //GameManager.Instance.audienceManager.SpawnNextAudience();
        }

        void RotateToDir(Vector3 targetPosition)
        {
            // if current direction is equal to target direction then don't rotate.
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0f;

            if (dir == Vector3.zero)
                return;

            // apply target rotation at once.
            transform.rotation = Quaternion.LookRotation(dir);
        }

        void Update()
        {
            if (audienceState != AudienceState.Walk)
            {
                if (GameManager.Instance.comedian)
                {
                    RotateToDir(GameManager.Instance.comedian.transform.position);
                }
                else
                {
                    RotateToDir(GameManager.Instance.audienceManager.lookPoint.position);
                }
            }
        }
    }
}
