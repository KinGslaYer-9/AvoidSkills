using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    private void Start()
    {
        var followCam = FindObjectOfType<CinemachineVirtualCamera>();        
        
        followCam.Follow = transform;
        followCam.LookAt = transform;
    }
}
