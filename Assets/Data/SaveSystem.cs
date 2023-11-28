using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class SaveSystem
{
    private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");

    public static void PrintFilePath()
    {
        Debug.Log(SaveFilePath);
    }
    
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
            level.name = allLevels.levels[i].name;
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
    
    public static void SaveCurrentLevelData(Level currentLevel, bool isUnlocked, bool didFinish, bool didPerfect)
    {
        // Load the existing game data
        GameData gameData = LoadData();

        // Ensure gameData is not null
        if (gameData == null)
        {
            Debug.LogError("SaveCurrentLevelData: No existing game data found.");
            return;
        }

        // Find the LevelData for the current level using a unique identifier, such as the level name
        LevelData levelToUpdate = gameData.levels.Find(level => level.name == currentLevel.name);

        if (levelToUpdate != null)
        {
            // Update the LevelData for the current level
            levelToUpdate.didFinish = didFinish; // Or whatever conditions you're updating
            levelToUpdate.isUnlocked = isUnlocked;
            // Repeat for any other fields you want to update...
            if (didPerfect && !levelToUpdate.didPerfect)
            {
                levelToUpdate.didPerfect = true;
            }
            // Save the updated game data
            SaveData(gameData);
        }
        else
        {
            Debug.LogError($"SaveCurrentLevelData: Level {currentLevel.name} not found in saved data.");
        }
    }
    
    public static LevelData GetLevelData(Level wantedLevel)
    {
        // Load the existing game data
        GameData gameData = LoadData();

        // Ensure gameData is not null
        if (gameData == null)
        {
            Debug.LogError("GetLevelData: No existing game data found.");
            return null;
        }

        // Find the LevelData for the given level name
        LevelData levelData = gameData.levels.Find(level => level.name == wantedLevel.name);

        if (levelData != null)
        {
            // Return the found LevelData
            return levelData;
        }
        else
        {
            Debug.LogWarning($"GetLevelData: Level {wantedLevel.name} not found in saved data.");
            return null; // Return null if the level data does not exist
        }
    }
}