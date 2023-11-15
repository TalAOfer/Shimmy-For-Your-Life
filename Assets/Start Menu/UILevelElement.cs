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
    [SerializeField] private Level level;

    public void SetButtonState(bool enable) => button.interactable = enable;

    public void SetLevel(Level correspondentLevel)
    {
        level = correspondentLevel;
        text.text = level.sceneName;
        gameObject.name = level.sceneName;
    }
    public void DoChangeScene() => ChangeScene.Raise(this, level);
}