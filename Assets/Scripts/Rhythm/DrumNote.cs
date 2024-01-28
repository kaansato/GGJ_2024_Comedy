using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;

namespace GGJFUK
{
    public enum NoteType
    {
        Up,
        Down,
        Left,
        Right,
        Length
    }

    public enum JokeType
    {
        Perfect,
        Great,
        Good,
        Bad,
        Length
    }

    public class DrumNote : MonoBehaviour
    {
        public GameObject[] noteImages;

        public KeyCode[] keys;

        public GameObject missImage;

        [HideInInspector]
        public bool isOverlaping = false;

        [HideInInspector]
        public KeyCode keyCode;

        RhythmGame rhythmGame;

        NoteType noteType;

        float distance;
        float lastScale;

        Move move = null;

        void Update()
        {

        }

        public void InitNote(RhythmGame rhythmGame)
        {
            Debug.Log("DrumNote InitNote");

            this.rhythmGame = rhythmGame;

            lastScale = transform.localScale.x;

            transform.localScale = Vector3.one;

            int randomValue = Random.Range(0, 4);

            noteType = (NoteType)randomValue;

            foreach (GameObject ni in noteImages)
            {
                ni.SetActive(false);
            }

            noteImages[randomValue].SetActive(true);

            keyCode = keys[randomValue];

            MoveNote();
        }

        public void MoveNote()
        {
            move = new Move(this.gameObject, rhythmGame.noteStartPosition, rhythmGame.noteEndPosition, 0, rhythmGame.moveTime,
                    tweenType: TransitionHelper.TweenType.linear, coordinateSpace: TransitionStep.CoordinateSpaceType.AnchoredPosition, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            move.Start();
        }

        void LogStart()
        {
            //Debug.Log("RhythmGame Start");
        }

        void LogUpdate(float progress)
        {
            //Debug.Log("RhythmGame Update:" + progress);

            if (isOverlaping && !IsOverlap())
            {
                Debug.Log("MISSED !");

                GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.missAudio);

                foreach (GameObject ni in noteImages)
                {
                    ni.SetActive(false);
                }

                missImage.SetActive(true);

                GameManager.Instance.IncreaseMissCount();

                // Reset Laugh Bar
                GameManager.Instance.laugh = 0;
                GameManager.Instance.uIManager.UpdateLaughBar();
            }

            isOverlaping = IsOverlap();

            //Debug.Log("IsOverlap: " + isOverlaping);
        }

        void LogComplete()
        {
            //Debug.Log("RhythmGame Complete");

            DestroyNote();
        }

        bool IsOverlap()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            //Debug.Log("rect1Rect: " + rectTransform.position + " " + rectTransform.sizeDelta);
            //Debug.Log("rect2Rect: " + rhythmGame.targetRectTransform.position + " " + rhythmGame.targetRectTransform.sizeDelta);

            Rect rect1Rect = new Rect(rectTransform.position, rectTransform.sizeDelta);
            Rect rect2Rect = new Rect(rhythmGame.targetRectTransform.position, rhythmGame.targetRectTransform.sizeDelta);

            distance = Vector3.Distance(rectTransform.position, rhythmGame.targetRectTransform.position) * lastScale;

            /*
            if (rect1Rect.Overlaps(rect2Rect))
            //if (distance < rhythmGame.targetRectTransform.sizeDelta.x) ;
            {
                Debug.Log("distance: " + distance + " " + rhythmGame.targetRectTransform.sizeDelta.x + " " + lastScale);
            }
            */

            //return rect1Rect.Overlaps(rect2Rect);

            return (distance < rhythmGame.targetRectTransform.sizeDelta.x);
        }

        public void DestroyNote()
        {
            rhythmGame.drumNotes.Remove(this);

            Destroy(this.gameObject);
        }

        public JokeType GetJokeType()
        {
            Debug.Log("GetPointForJoke: " + distance);

            if (distance <= 5)
            {
                return JokeType.Perfect;
            }
            else if (distance <= 30)
            {
                return JokeType.Great;
            }
            else if (distance <= 80)
            {
                return JokeType.Good;
            }
            else
            {
                return JokeType.Bad;
            }
        }

        public void Stop()
        {
            move.Stop();
            move = null;
        }

        public void Shrink()
        {
            Vector3 startPosition = GetComponent<RectTransform>().anchoredPosition;
            Vector3 endPosition = rhythmGame.targetRectTransform.anchoredPosition;

            Stop();

            move = new Move(this.gameObject, startPosition, endPosition, 0, 0.2f,
                    tweenType: TransitionHelper.TweenType.linear, coordinateSpace: TransitionStep.CoordinateSpaceType.AnchoredPosition);

            move.Start();

            Scale scale = new Scale(this.gameObject, Vector3.one, Vector3.zero, 0, 0.2f,
                tweenType: TransitionHelper.TweenType.linear, onComplete: DestroyNote);

            scale.Start();
        }
    }
}
