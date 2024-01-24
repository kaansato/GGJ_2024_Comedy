using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using FlipWebApps.BeautifulTransitions.Scripts.DisplayItem;
using UnityEngine.UI;

namespace GGJFUK
{
    public class RhythmGame : MonoBehaviour
    {
        public Transform parentTransform;

        public RectTransform targetRectTransform;

        public GameObject notePrefab;

        public float minSpawnInterval = 1f;
        public float maxSpawnInterval = 3f;

        public float moveTime = 5f;

        public Vector3 noteStartPosition = new Vector3(500f, 10f, 0f);
        public Vector3 noteEndPosition = new Vector3(-500f, 10f, 0f);

        [HideInInspector]
        public List<DrumNote> drumNotes = new List<DrumNote>();

        public TextMeshProUGUI countdownText;
        int countdownValue = 3;

        public TextMeshProUGUI infoText;
        float hideDelay = 1f;
        float timer;

        bool isPlaying = true;
        bool isFirstTime = true;

        void Start()
        {
            //StartGame();
        }

        public void StartCountdown()
        {
            HideInfo();

            countdownValue = 3;

            StartCoroutine(Countdown());
        }

        IEnumerator Countdown()
        {
            while (countdownValue > 0)
            {
                countdownText.text = countdownValue.ToString();

                yield return new WaitForSeconds(1f);
                
                countdownValue--;
            }

            countdownText.text = "GO!";
        }

        public void StartGame()
        {
            Debug.Log("RhythmGame StartGame");

            countdownText.text = "";

            isPlaying = true;
            isFirstTime = true;

            StartCoroutine(SpawnNotes());
        }

        public void StopGame()
        {
            Debug.Log("RhythmGame StopGame");

            isPlaying = false;

            foreach(DrumNote d in drumNotes)
            {
                d.Stop();
            }

            for (int i = drumNotes.Count - 1; i >= 0; i--)
            {
                DrumNote drumNote = drumNotes[i];

                drumNote.DestroyNote();
            }

            drumNotes.Clear();
        }

        public void ShowInfo(string info)
        {
            infoText.text = info;
            DisplayItemHelper.SetAttention(infoText.gameObject, true);

            timer = 0;
        }

        void HideInfo()
        {
            infoText.text = "";
            DisplayItemHelper.SetAttention(infoText.gameObject, false);
        }

        IEnumerator SpawnNotes()
        {
            while (isPlaying)
            {
                if(!isFirstTime)
                {
                    yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
                }

                isFirstTime = false;

                if (!isPlaying)
                {
                    Debug.Log("isPlaying False");

                    break;
                }

                GameObject noteObject = Instantiate(notePrefab, noteStartPosition, Quaternion.identity);
                noteObject.transform.SetParent(parentTransform);

                DrumNote drumNote = noteObject.GetComponent<DrumNote>();
                drumNote.InitNote(this);
                drumNotes.Add(drumNote);
            }
        }

        void Update()
        {
            CheckNotes();

            if (infoText.text.Length > 0)
            {
                timer += Time.deltaTime;

                if (timer >= hideDelay)
                {
                    HideInfo();

                    timer = 0;
                }
            }
        }

        void CheckNotes()
        {
            for (int i = drumNotes.Count - 1; i >= 0; i--)
            {
                DrumNote drumNote = drumNotes[i];

                if (Input.GetKeyDown(drumNote.keyCode) && drumNote.isOverlaping)
                {
                    Debug.Log("CORRECT !");

                    JokeType jokeType = drumNote.GetJokeType();

                    drumNote.Shrink();

                    int pointForJoke = 100;
                    int laughForJoke = 20;

                    if(jokeType == JokeType.Perfect)
                    {
                        ShowInfo("PERFECT");
                        pointForJoke = 100;
                        laughForJoke = 20;
                    }
                    else if (jokeType == JokeType.Good)
                    {
                        ShowInfo("GOOD");
                        pointForJoke = 80;
                        laughForJoke = 18;
                    }
                    else if (jokeType == JokeType.Ok)
                    {
                        ShowInfo("OK");
                        pointForJoke = 60;
                        laughForJoke = 16;
                    }

                    GameManager.Instance.score += pointForJoke;
                    GameManager.Instance.uIManager.UpdateScore();

                    GameManager.Instance.laugh += laughForJoke;
                    GameManager.Instance.uIManager.UpdateLaughBar();

                    if (GameManager.Instance.CheckLaugh()) return;

                    GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.correctAudio);
                }
            }
        }
    }
}
