using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //[SerializeField, HideInInspector]
    public List<LevelData> levels = new();

    // Public property to access levels
    //public List<LevelData> Levels { get; set; }

}

[System.Serializable]
public class LevelData
{
    public string name;
    public bool didFinish = false;
    public bool isUnlocked = false;
    public bool didPerfect = false;
}