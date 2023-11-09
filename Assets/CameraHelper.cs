using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;
public class CameraHelper : MonoBehaviour
{
    public PlayerController player;
    public CinemachineVirtualCamera virtualCam;
    public Vector3 twoDFollowOffset;
    public Vector3 threeDFollowOffset;
    public Vector3 threeDRotation;
    public Vector3 threeDPlayerRotation;
        
    [Button]
    public void ChangeTo2D()
    {
        Camera.main.orthographic = true;
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = twoDFollowOffset;
        virtualCam.transform.eulerAngles = Vector3.zero;
        player.transform.eulerAngles = Vector3.zero;
        player.is3D = false;
    }

    [Button]
    public void ChangeTo3D()
    {
        Camera.main.orthographic = false;
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = threeDFollowOffset;
        virtualCam.transform.eulerAngles = threeDRotation;
        player.transform.eulerAngles = threeDPlayerRotation;
        player.is3D = true;
    }
}
