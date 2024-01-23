using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        bool isPlaying = true;

        void Start()
        {
            //StartGame();
        }

        public void StartCountdown()
        {
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
            countdownText.text = "";

            isPlaying = true;

            StartCoroutine(SpawnNotes());
        }

        public void StopGame()
        {
            isPlaying = false;

            for (int i = drumNotes.Count - 1; i >= 0; i--)
            {
                DrumNote drumNote = drumNotes[i];

                drumNote.DestroyNote();
            }

            drumNotes.Clear();
        }

        IEnumerator SpawnNotes()
        {
            while (isPlaying)
            {
                yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

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
        }

        void CheckNotes()
        {
            for (int i = drumNotes.Count - 1; i >= 0; i--)
            {
                DrumNote drumNote = drumNotes[i];

                if (Input.GetKeyDown(drumNote.keyCode) && drumNote.isOverlaping)
                {
                    Debug.Log("CORRECT !");

                    drumNote.DestroyNote();

                    GameManager.Instance.score += drumNote.pointForJoke;
                    GameManager.Instance.uIManager.UpdateScore();

                    GameManager.Instance.laugh += GameManager.Instance.laughForJoke;
                    GameManager.Instance.uIManager.UpdateLaughBar();

                    if (GameManager.Instance.CheckLaugh()) return;

                    GameManager.Instance.audioManager.PlayAudio(GameManager.Instance.audioManager.correctAudio);
                }
            }
        }
    }
}
