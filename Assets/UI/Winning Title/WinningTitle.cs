using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinningTitle : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButton, nextLevelButton, perfectSign;
    [SerializeField] private BoolVariable didPerfect;
    
    public void EnableButtons()
    {
        mainMenuButton.SetActive(true);
        perfectSign.SetActive(didPerfect.value);
        nextLevelButton.SetActive(true);
    }
}