using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Tools
{
    public static Vector2 GetDirectionVector(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return Vector2.up;
            case Directions.Right:
                return Vector2.right;
            case Directions.Down:
                return Vector2.down;
            case Directions.Left:
                return Vector2.left;
            case Directions.UpRight:
                return new Vector2(1, 1); // Diagonal up-right
            case Directions.UpLeft:
                return new Vector2(-1, 1); // Diagonal up-left
            case Directions.DownRight:
                return new Vector2(1, -1); // Diagonal down-right
            case Directions.DownLeft:
                return new Vector2(-1, -1); // Diagonal down-left
            // Add cases for any other directions you need
            default:
                return Vector2.zero;
        }
    }

    public static float GetIntervalLengthFromBPM(Song song, int everyXBeats)
    {
        return (60f / song.bpm) * everyXBeats;
    }


    public static List<int> GetRandoms(int numOfNums, int min, int max)
    {
        if (max - min + 1 < numOfNums)
        {
            throw new System.Exception("Range is too small for the number of unique numbers requested.");
        }

        List<int> randomNumbers = new List<int>();
        while (randomNumbers.Count < numOfNums)
        {
            int randomNumber = Random.Range(min, max + 1);
            if (!randomNumbers.Contains(randomNumber))
            {
                randomNumbers.Add(randomNumber);
            }
        }

        return randomNumbers;
    }
    
    public static void PlaySound(string soundName, Vector3 pos)
    {
        string eventName = "event:/" + soundName;
        FMODUnity.RuntimeManager.PlayOneShot(eventName, pos);
    }

    public static bool didSucceed(float chance)
    {
        int rand = Random.Range(0, 100);
        return (chance > rand);
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position)
    {
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instance.transform.position = position;
        return instance;
    }
}

public enum Directions
{
    Up,
    Right,
    Down,
    Left,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft,
    InPlace
}