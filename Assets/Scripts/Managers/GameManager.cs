using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJFUK
{
    public class GameManager : Singleton<GameManager>
    {
        public int score = 0;
        
        public int missCount = 0;
        public int maxMissCount = 3;

        public int laugh = 0;
        public int maxLaugh = 100;
        public int laughForJoke = 20;

        public UIManager uIManager;
        public AudioManager audioManager;

        public RhythmGame rhythmGame;

        public GameObject[] comedians;

        public GameObject[] audiences;

        public Gate[] gates;

        Comedian comedian = null;

        int comedianIndex = 0;

        void Start()
        {
            Invoke("SpawnComedian", 3.0f);
        }

        void SpawnComedian()
        {
            Vector3 spawnPosition = new Vector3 (0.1f, -17.6f, -6.5f);

            GameObject comedianObject = Instantiate(comedians[comedianIndex], spawnPosition, Quaternion.identity);

            comedian = comedianObject.GetComponent<Comedian>();

            SetAudienceAnimation(1);

            audioManager.PlayAudio(audioManager.applauseAudio);

            rhythmGame.gameObject.SetActive(true);
            rhythmGame.StartCountdown();
        }

        public void SetAudienceAnimation(int state)
        {
            foreach (GameObject o in audiences) 
            {
                Animator animator = o.GetComponent<Animator>();

                animator.SetInteger("State", state);
            }
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
            CloseGates();

            rhythmGame.moveTime -= 0.5f;

            comedianIndex++;
            if (comedianIndex > 3)
                comedianIndex = 0;

            Invoke("SpawnComedian", 1.0f);
        }

        void StartLaugh()
        {
            audioManager.StopAll();

            audioManager.PlayAudio(audioManager.laughAudio);
            SetAudienceAnimation(2);

            Invoke("StopLaugh", 3.0f);
        }

        public void StopLaugh()
        {
            audioManager.StopAll();

            SetAudienceAnimation(0);
        }

        public void IncreaseMissCount()
        {
            missCount++;

            missCount = Mathf.Clamp(missCount, 0, maxMissCount);

            if(missCount == maxMissCount)
            {
                Debug.Log("GAME OVER !");
            }

            uIManager.ShowMissCount();
        }

        public bool CheckLaugh()
        {
            if (laugh > maxLaugh)
            {
                laugh = 0;
                uIManager.UpdateLaughBar();

                uIManager.laughSlider.gameObject.SetActive(false);

                rhythmGame.StopGame();
                rhythmGame.gameObject.SetActive(false);

                StartLaugh();

                comedian.StartWin();

                return true;
            }

            return false;
        }

        void Update()
        {

        }
    }
}
