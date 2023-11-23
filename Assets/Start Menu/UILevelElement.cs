using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelElement : MonoBehaviour
{
    [SerializeField] private GameEvent ChangeScene;
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Level level;
    [SerializeField] private Button button;
    [SerializeField] private Image perfectIcon;
    [SerializeField] private Sprite fullStar, emptyStar;

    public void SetButtonState(bool enable)
    {
        button.interactable = enable;
        perfectIcon.enabled = enable;
    }
        
    public void SetPerfectState(bool perfect) => perfectIcon.sprite = perfect ? fullStar : emptyStar;

    public void SetLevel(Level correspondentLevel)
    {
        level = correspondentLevel;
        text.text = level.sceneName;
        gameObject.name = level.sceneName;
    }
    public void DoChangeScene() => ChangeScene.Raise(this, level);
}