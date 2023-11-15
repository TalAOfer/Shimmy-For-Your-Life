using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;
public class CameraHelper : MonoBehaviour
{
    [SerializeField] private GameObject player, winFlag;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private Vector3 twoDFollowOffset;
    [SerializeField] private Vector3 threeDFollowOffset;
    [SerializeField] private Vector3 threeDCamRotation;
    [SerializeField] private Vector3 threeDObjectRotation;
    [SerializeField] private BoolVariable is3d;
        
    [Button]
    public void ChangeTo2D()
    {
        Camera.main.orthographic = true;
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = twoDFollowOffset;
        virtualCam.transform.eulerAngles = Vector3.zero;
        player.transform.eulerAngles = Vector3.zero;
        winFlag.transform.eulerAngles = Vector3.zero;
        is3d.value = false;
    }

    [Button]
    public void ChangeTo3D()
    {
        Camera.main.orthographic = false;
        virtualCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = threeDFollowOffset;
        virtualCam.transform.eulerAngles = threeDCamRotation;
        player.transform.eulerAngles = threeDObjectRotation;
        winFlag.transform.eulerAngles = threeDObjectRotation;
        is3d.value = true;
    }
}
