using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MoveCard : MonoBehaviour
{
    [SerializeField] private Sprite nonPressedKey;
    [SerializeField] private Sprite pressedKey;

    [SerializeField] private Image numberKeyImage;
    [SerializeField] private Image moveImage;

    public void Init(Sprite moveIcon)
    {
        moveImage.sprite = moveIcon;
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
