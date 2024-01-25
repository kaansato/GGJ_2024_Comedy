using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCameraBase mainCamera;
    public List<CinemachineVirtualCameraBase> cameras;
    
    public bool rotateMode;
    private int cameraIndex;
    
    void Start()
    {
        cameraIndex = 0;
        rotateMode = false;

        ResetPosition();
    }
    
    // カメラの位置をリセットする
    public void ResetPosition()
    {
        rotateMode = false;
        mainCamera.Priority = 1;
        cameras.ForEach(c => c.Priority = 0);
    }
    
    public void RotationMode()
    {
        rotateMode = true;
    }
    
    // カメラの位置を回転させる
    private void rotatePosition()
    {
        mainCamera.Priority = 0;
        for (int i = 0; i < cameras.Count; i++)
        {
            if (i == cameraIndex)
            {
                cameras[i].Priority = 1;
            }
            else 
            {
                cameras[i].Priority = 0;
            }
        }
        cameraIndex = (cameraIndex + 1) % cameras.Count;
    }

    void Update()
    {
        if (rotateMode == true)
        {
            // 7秒ごとにカメラを切り替える. Magic Numberになっているので，要修正．
            if (Time.frameCount % 420 == 0)
            {
                rotatePosition();
            }

        }
    }
}