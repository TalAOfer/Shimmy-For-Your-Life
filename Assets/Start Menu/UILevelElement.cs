using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelElement : MonoBehaviour
{
    [SerializeField] private GameEvent ChangeScene;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int levelIndex;

    public void SetButtonState(bool enable) => button.interactable = enable;

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    public void SetLevelName(string levelName) => text.text = levelName;
    public void DoChangeScene() => ChangeScene.Raise(this, levelIndex);
}