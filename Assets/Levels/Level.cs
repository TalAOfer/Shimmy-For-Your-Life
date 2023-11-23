using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[CreateAssetMenu(menuName = "Level")]
[Serializable]
public class Level : ScriptableObject
{
    public string sceneName;
    public Song defaultSong;
    public List<Move> playerMoves;

#if UNITY_EDITOR

    public void GotoScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }
#endif
}