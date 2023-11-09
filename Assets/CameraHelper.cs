using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CameraHelper : MonoBehaviour
{
    public PlayerController player;
    public Vector3 twoDPos;
    public Vector3 threeDPos;
    public Vector3 threeDRotation;
    public Vector3 threeDPlayerRotation;
        
    [Button]
    public void ChangeTo2D()
    {
        Camera.main.orthographic = true;
        transform.position = twoDPos;
        transform.eulerAngles = Vector3.zero;
        player.transform.eulerAngles = Vector3.zero;
        player.is3D = false;
    }

    [Button]
    public void ChangeTo3D()
    {
        Camera.main.orthographic = false;
        transform.position = threeDPos;
        transform.eulerAngles = threeDRotation;
        player.transform.eulerAngles = threeDPlayerRotation;
        player.is3D = true;
    }
}
