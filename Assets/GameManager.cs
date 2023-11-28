using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoolVariable didPerfect;
    [SerializeField] private GameEvent initializeLevel;
    [SerializeField] private Level currentLevel;
    [SerializeField] private IntVariable playerActiveMove;

    void Start()
    {
        //Reset global variables
        playerActiveMove.value = -1;
        didPerfect.value = true;
        //
        initializeLevel.Raise(this, currentLevel);
    }


    public void OnPlayerMissedBeat()
    {
        didPerfect.value = false;
    }

    public void OnWin()
    {
        UpdateCurrentLevelState();
        UpdateNextLevelState();
    }

    private void UpdateCurrentLevelState()
    {
        SaveSystem.SaveCurrentLevelData(currentLevel, true, true, didPerfect.value);
    }

    private void UpdateNextLevelState()
    {
        if (currentLevel.unlockWhenFinished != null)
        {
            LevelData nextLevelData = SaveSystem.GetLevelData(currentLevel.unlockWhenFinished);

            if (!nextLevelData.isUnlocked)
            {
                SaveSystem.SaveCurrentLevelData(currentLevel.unlockWhenFinished, true, false, false);
            }
        }
    }
}