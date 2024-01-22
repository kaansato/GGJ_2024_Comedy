using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJFUK
{
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uIManager;
        public AudioManager audioManager;

        public GameObject comedianPrefab;

        public GameObject[] audiences;

        public Gate[] gates;

        Comedian comedian = null;

        void Start()
        {
            Invoke("SpawnComedian", 3.0f);
        }

        void SpawnComedian()
        {
            Vector3 spawnPosition = new Vector3 (0.1f, -17.6f, -6.5f);

            GameObject comedianObject = Instantiate(comedianPrefab, spawnPosition, Quaternion.identity);

            comedian = comedianObject.GetComponent<Comedian>();

            SetAudienceAnimation(1);

            audioManager.PlayAudio(audioManager.applauseAudio);
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

        void Update()
        {

        }
    }
}
