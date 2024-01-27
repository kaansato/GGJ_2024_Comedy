using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJFUK
{
    public class TitleManager : MonoBehaviour
    {
        public Button startButton;
        public Transform titleCamera;

        public float tebureSpeed = 1f;
        public float tebureAmount = 0.1f;

        void Start()
        {
            startButton.onClick.AddListener(() =>
            {
                //StartCoroutine(OnStartButtonClicked());

                OnStartButtonClicked();
            });
        }

        void Update()
        {
            Tebure();
        }

        /*
        public IEnumerator OnStartButtonClicked()
        {
            // 2秒待つ
            yield return new WaitForSeconds(1f);

            audioManager.PlayAudio(audioManager.laughAudio);
            SceneManager.LoadScene("Game");
        }
        */

        void OnStartButtonClicked()
        {
            GameManager.Instance.StartGame();
        }

        // カメラを時間に沿って、ゆっくりなめらかに手ブレさせる
        void Tebure()
        {
            if (titleCamera.gameObject.activeSelf && titleCamera.parent.gameObject.activeSelf)
            {
                float y = Mathf.Sin(Time.time * tebureSpeed) * tebureAmount;
                titleCamera.localPosition = new Vector3(0f, y, 0f);
            }
        }
    }
}
