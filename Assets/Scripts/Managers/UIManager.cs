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
    }
}
