using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam1;
    public CinemachineVirtualCameraBase vcam2;
    
    public void ResetPosition()
    {
        vcam1.Priority = 1;
        vcam2.Priority = 0;
    }
    
    public void ChangePosition()
    {
        vcam1.Priority = 0;
        vcam2.Priority = 1;
    }

    void Start()
    {
        ResetPosition();
    }

    void Update()
    {
    }
}