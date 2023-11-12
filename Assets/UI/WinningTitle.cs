using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinningTitle : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButton, nextLevelButton;

    public void EnableButton()
    {
        mainMenuButton.SetActive(true);
        nextLevelButton.SetActive(true);
    }
}