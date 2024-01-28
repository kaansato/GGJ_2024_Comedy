using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJFUK
{
    public class GameManager : Singleton<GameManager>
    {
        public int score = 0;
        
        public int missCount = 0;
        public int maxMissCount = 3;

        public int laugh = 0;
        public int maxLaugh = 100;

        public int stage = 0;

        public int correctCount = 0;

        public UIManager uIManager;
        public AudioManager audioManager;
        public AudienceManager audienceManager;

        public RhythmGame rhythmGame;

        public CameraManager cameraManager;

        public GameObject[] comedians;

        public GameObject gameCamera;
        public GameObject titleCamera;

        //public GameObject[] audiences;

        public Gate[] gates;

        [HideInInspector]
        public Comedian comedian = null;

        int comedianIndex = 0;

        void Start()
        {
            Time.timeScale = 1f;

            //Invoke("SpawnComedian", 3.0f);
        }

        public void StartGame()
        {
            titleCamera.SetActive(false);
            gameCamera.SetActive(true);

            audioManager.PlayAudio(audioManager.startAudio);

            audioManager.PlayMusic(audioManager.gameAudio, 0.2f);

            uIManager.ShowGameScreen();

            Invoke("SpawnComedian", 2.0f);
        }

        void SpawnComedian()
        {
            cameraManager.ResetPosition();

            Vector3 spawnPosition = new Vector3 (0.1f, -17.6f, -6.5f);

            GameObject comedianObject = Instantiate(comedians[comedianIndex], spawnPosition, Quaternion.identity);

            comedian = comedianObject.GetComponent<Comedian>();

            audienceManager.SetAudienceState(AudienceState.Clap);

            audioManager.PlayAudio(audioManager.applauseAudio);

            rhythmGame.gameObject.SetActive(true);
            rhythmGame.StartCountdown();

            cameraManager.RotationMode();
        }

        public void OpenGates()
        {
            foreach(Gate g in gates)
            {
                g.OpenGate();
            }
        }

        public void CloseGates()
        {
            foreach (Gate g in gates)
            {
                g.CloseGate();
            }
        }

        public void EndStage()
        {
            audioManager.PlayAudio(audioManager.clearAudio);

            cameraManager.ResetPosition();

            CloseGates();

            stage++;

            if(stage % 2 == 0)
            {
                rhythmGame.minSpawnInterval = 0.5f;
                rhythmGame.maxSpawnInterval -= 0.5f;

                if(rhythmGame.maxSpawnInterval < 1f)
                {
                    rhythmGame.maxSpawnInterval = 1f;
                }
            }
            else
            {
                rhythmGame.moveTime -= 0.5f;

                if(rhythmGame.moveTime < 1f)
                {
                    rhythmGame.moveTime = 1f;
                }
            }

            maxLaugh += 10;

            comedianIndex++;
            if (comedianIndex >= comedians.Length)
                comedianIndex = 0;

            Invoke("SpawnComedian", 1.0f);
        }

        void StartLaugh()
        {
            audioManager.StopAll();

            audioManager.PlayAudio(audioManager.laughAudio);

            audienceManager.SetAudienceState(AudienceState.Laugh);

            Invoke("StopLaugh", 3.0f);
        }

        public void StopLaugh()
        {
            audioManager.StopAll();

            audienceManager.SetAudienceState(AudienceState.Idle);
        }

        public void IncreaseMissCount()
        {
            missCount++;

            missCount = Mathf.Clamp(missCount, 0, maxMissCount);

            if(missCount == maxMissCount)
            {
                Debug.Log("GAME OVER !");

                rhythmGame.StopGame();
                rhythmGame.gameObject.SetActive(false);

                cameraManager.ResetPosition();

                Invoke("GameOver", 2f);
            }

            uIManager.ShowMissCount();
        }

        void GameOver()
        {
            audioManager.StopAll();

            audioManager.StopMusic();

            audioManager.PlayLoopAudio(audioManager.gameOverAudio, 0.5f);

            GameManager.Instance.uIManager.ShowGameOver();
        }

        public bool CheckLaugh()
        {
            if (laugh > maxLaugh)
            {
                laugh = 0;
                uIManager.UpdateLaughBar();

                uIManager.laughSlider.gameObject.SetActive(false);

                /*
                rhythmGame.StopGame();
                rhythmGame.gameObject.SetActive(false);
                */

                rhythmGame.StopGame();
                Invoke("DoLater", 1.0f);

                StartLaugh();

                comedian.StartWin();

                cameraManager.ResetPosition();

                return true;
            }

            return false;
        }

        void DoLater()
        {
            rhythmGame.gameObject.SetActive(false);
        }

        public void Restart()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
