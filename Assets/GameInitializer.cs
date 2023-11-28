using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private BoolVariable wasGameInitialized;
    [SerializeField] private AllLevels allLevels;

    // Start is called before the first frame update
    void Start()
    {
            if (!wasGameInitialized.value)
            {
                wasGameInitialized.value = true;
                InitializeSaveSystem();
            }
    }

    public void InitializeSaveSystem()
    {
        GameData currentGameData = SaveSystem.LoadData();

        if (currentGameData == null)
        {
            currentGameData = SaveSystem.CreateNewData(allLevels);
        }
    }
}
