using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJFUK
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public Slider laughSlider;

        public GameObject[] crossImages;

        public GameObject gameover;
        public TextMeshProUGUI endScoreText;
        public TextMeshProUGUI bestScoreText;

        public GameObject titleImage;
        public GameObject startButton;

        public void ShowGameScreen()
        {
            titleImage.SetActive(false);
            startButton.SetActive(false);

            scoreText.gameObject.SetActive(true);

            foreach (GameObject ci in crossImages)
            {
                ci.transform.parent.gameObject.SetActive(true);
            }
        }

        public void UpdateScore()
        {
            scoreText.text = "SCORE: " + GameManager.Instance.score;
        }

        public void UpdateLaughBar()
        {
            float sliderValue = (float)GameManager.Instance.laugh / GameManager.Instance.maxLaugh;

            sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);

            laughSlider.value = sliderValue;
        }

        public void ShowMissCount()
        {
            foreach (GameObject ci in crossImages) 
            {
                ci.SetActive(false);
            }

            for (int i = 0; i < GameManager.Instance.missCount; i++)
            {
                if (i < crossImages.Length)
                {
                    crossImages[i].SetActive(true);
                }
            }
        }

        public void ShowGameOver()
        {
            // Hide All
            scoreText.gameObject.SetActive(false);
            laughSlider.gameObject.SetActive(false);
            GameManager.Instance.rhythmGame.gameObject.SetActive(false);

            Time.timeScale = 0f;

            gameover.SetActive(true);

            int score = GameManager.Instance.score;

            endScoreText.text = "SCORE: " + score;

            int bestScore = PlayerPrefs.GetInt("BEST_SCORE", 0);

            Debug.Log("GAME OVER: " + score + " " + bestScore);

            if(score >= bestScore)
            {
                bestScoreText.text = "BEST SCORE !";

                PlayerPrefs.SetInt("BEST_SCORE", score);
                PlayerPrefs.Save();
            }
            else
            {
                bestScoreText.text = "BEST SCORE: " + bestScore;
            }
        }
    }
}
