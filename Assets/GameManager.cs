using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoolVariable didPerfect;
    [SerializeField] private GameEvent initializeLevel;
    [SerializeField] private Level currentLevel;
    [SerializeField] private DanceFloorManager danceFloorManager;
    void Start()
    {
        didPerfect.value = true;
        initializeLevel.Raise(this, currentLevel);
    }

    public void OnPlayerMissedBeat()
    {
        didPerfect.value = false;
    }
    
    
}
