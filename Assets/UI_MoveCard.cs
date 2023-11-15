using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MoveCard : MonoBehaviour
{
    [SerializeField] private Color pickedColor, nonPickedColor;
    [SerializeField] private Sprite nonPressedKey;
    [SerializeField] private Sprite pressedKey;

    [SerializeField] private Image numberKeyImage;
    [SerializeField] private Image moveImage;
    [SerializeField] private Image BG;
    public RectTransform rectTransform;
    public bool isActive;
    
        //TODO: init the Num also from outside
    
    public void SetIcon(Sprite moveIcon)
    {
        moveImage.sprite = moveIcon;
    }

    public void SetPicked(bool isPicked)
    {
        if (isPicked)
        {
            isActive = true;
            numberKeyImage.sprite = pressedKey;
            BG.color = pickedColor;
        }
        else
        {
            isActive = false;
            numberKeyImage.sprite = nonPressedKey;
            BG.color = nonPickedColor;
        }
    }
    
    public void OnValueChange(bool isPressed)
    {
        if (isPressed)
        {
            numberKeyImage.sprite = pressedKey;
        } else
        {
            numberKeyImage.sprite = nonPressedKey;
        }
    }
}
