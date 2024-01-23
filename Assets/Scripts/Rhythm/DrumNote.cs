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

    public class DrumNote : MonoBehaviour
    {
        public int pointForJoke = 100;

        public GameObject[] noteImages;

        public KeyCode[] keys;

        public GameObject missImage;

        [HideInInspector]
        public bool isOverlaping = false;

        [HideInInspector]
        public KeyCode keyCode;

        RhythmGame rhythmGame;

        NoteType noteType;

        void Update()
        {

        }

        public void InitNote(RhythmGame rhythmGame)
        {
            this.rhythmGame = rhythmGame;

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
            Move move = new Move(this.gameObject, rhythmGame.noteStartPosition, rhythmGame.noteEndPosition, 0, rhythmGame.moveTime,
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

                missImage.SetActive(true);

                GameManager.Instance.IncreaseMissCount();
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

            /*
            float distance = Vector3.Distance(rectTransform.position, rhythmGame.targetRectTransform.position);
            Debug.Log("Distance: " + distance);
            */

            return rect1Rect.Overlaps(rect2Rect);
        }

        public void DestroyNote()
        {
            rhythmGame.drumNotes.Remove(this);

            Destroy(this.gameObject);
        }
    }
}
