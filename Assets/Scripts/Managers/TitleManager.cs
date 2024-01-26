using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public GGJFUK.AudioManager audioManager;
    public Transform camera;

    public float tebureSpeed = 1f;
    public float tebureAmount = 0.1f;
    
    void Start()
    {
        startButton.onClick.AddListener(()=>
        {
            StartCoroutine(OnStartButtonClicked());
        });
    }

    void Update()
    {
        Tebure();
    }

    public IEnumerator OnStartButtonClicked()
    {
        // 2秒待つ
        yield return new WaitForSeconds(1f);

        audioManager.PlayAudio(audioManager.laughAudio);
        SceneManager.LoadScene("Game");
    }

    // カメラを時間に沿って、ゆっくりなめらかに手ブレさせる
    void Tebure()
    {
        float y = Mathf.Sin(Time.time * tebureSpeed) * tebureAmount;
        camera.localPosition = new Vector3(0f, y, 0f);
    }

}
