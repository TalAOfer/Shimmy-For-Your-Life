using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class WinningTitle : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButton, nextLevelButton, perfectSign;
    [SerializeField] private BoolVariable didPerfect;

    public void OnEnable()
    {
        perfectSign.SetActive(didPerfect.value);
    }
    public void EnableButtons()
    {
        mainMenuButton.SetActive(true);
        nextLevelButton.SetActive(true);
    }
}