using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<LevelData> levels = new();
}

[System.Serializable]
public class LevelData
{
    public string name;
    public bool didFinish = false;
    public bool isUnlocked = false;
    public bool didPerfect = false;
}