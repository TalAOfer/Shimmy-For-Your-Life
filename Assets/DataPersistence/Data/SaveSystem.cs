using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class SaveSystem
{
    private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");

    public static void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SaveFilePath, json);
    }

    public static GameData LoadData()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null; // Return null if no save file exists
    }

    public static GameData CreateNewData(AllLevels allLevels)
    {
        List<LevelData> levelDatas = new List<LevelData>();

        for (int i = 0; i < allLevels.levels.Count; i++)
        {
            LevelData level = new LevelData();
            // Unlock the first level.
            level.isUnlocked = (i == 0);
            level.didFinish = false;
            level.didPerfect = false;
            levelDatas.Add(level); // Add the created level data to the list
        }

        GameData newGameData = new GameData();
        newGameData.levels = levelDatas;

        SaveData(newGameData);
        return newGameData;
    }
    
    public static void ResetGameData()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
        }
    }
}