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

        //public TextMeshProUGUI infoText;

        public GameObject infoImage;
        public GameObject[] infoImages;

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

        public void ShowInfo(JokeType jokeType)
        {
            //infoText.text = info;

            infoImage.SetActive(true);

            DisplayItemHelper.SetAttention(infoImage, true);

            foreach (GameObject g in infoImages)
            {
                g.SetActive(false);
            }

            infoImages[(int)jokeType].SetActive(true);

            timer = 0;
        }

        void HideInfo()
        {
            //infoText.text = "";

            infoImage.SetActive(false);

            DisplayItemHelper.SetAttention(infoImage, false);
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

                //break;
            }
        }

        void Update()
        {
            CheckNotes();

            if (infoImage.activeSelf)
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

                    ShowInfo(jokeType);

                    if (jokeType == JokeType.Perfect)
                    {
                        pointForJoke = 100;
                        laughForJoke = 20;
                    }
                    else if (jokeType == JokeType.Great)
                    {
                        pointForJoke = 80;
                        laughForJoke = 18;
                    }
                    else if (jokeType == JokeType.Good)
                    {
                        pointForJoke = 60;
                        laughForJoke = 16;
                    }
                    else if (jokeType == JokeType.Bad)
                    {
                        pointForJoke = 40;
                        laughForJoke = 14;
                    }

                    GameManager.Instance.score += pointForJoke;
                    GameManager.Instance.uIManager.UpdateScore();

                    GameManager.Instance.laugh += laughForJoke;
                    GameManager.Instance.uIManager.UpdateLaughBar();

                    // Add audience for every 3 correct
                    GameManager.Instance.correctCount++;
                    if(GameManager.Instance.correctCount % 3 == 0)
                    {
                        GameManager.Instance.audienceManager.SpawnNextAudience();
                    }

                    if (GameManager.Instance.CheckLaugh()) return;

                    GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.correctAudio);
                }
            }
        }
    }
}
