using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "Level")]
[Serializable]
public class Level : ScriptableObject
{
    public string sceneName;
    public Song defaultSong;
    public List<Move> moves;

    [Button("Go To Scene", ButtonSizes.Large)]
    public void GotoScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
