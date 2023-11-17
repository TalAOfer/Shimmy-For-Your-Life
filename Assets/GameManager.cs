using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoolVariable didPerfect;
    void Start()
    {
        didPerfect.value = true;
    }

    public void OnPlayerMissedBeat()
    {
        didPerfect.value = false;
    }
}
