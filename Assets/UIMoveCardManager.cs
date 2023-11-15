using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveCardManager : MonoBehaviour
{
    [SerializeField] private CurrentLevel currentLevel;
    [SerializeField] private List<UI_MoveCard> moveCards;
    [SerializeField] private IntVariable playerActiveMove;
    private int currentActiveCard = 0;
    private float startY;
    private float endY = -70f;

    private void Start()
    {
        startY = moveCards[0].rectTransform.anchoredPosition.y;
        StartCoroutine(ShowCards(1f));
    }

    private void Update()
    {
        int activeMove = playerActiveMove.value - 1;
        for( int i = 0; i < moveCards.Count ; i++)
        {
            if (i == activeMove)
            {
                if (!moveCards[i].isActive)
                {
                    moveCards[i].SetPicked(true);
                }
            }
            else
            {
                if (moveCards[i].isActive)
                {
                    moveCards[i].SetPicked(false);
                }
            }
        }
    }


    // public void OnMarker(Component sender, object data)
    // {
    //     string markerString = (string)data;
    //     Debug.Log(markerString);
    //     if (markerString == "3")
    //     {
    //         StartCoroutine(ShowCards());
    //     }
    // }

    private IEnumerator ShowCards(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        float beatLength = Tools.GetIntervalLengthFromBPM(currentLevel.value.defaultSong, 1);
        float brakeLength = beatLength / 4f;
        
        for (int i = 0; i < moveCards.Count ; i++)
        {
            UI_MoveCard card = moveCards[i];
            card.SetIcon(currentLevel.value.playerMoves[i+1].icon);
            StartCoroutine(LerpYCoroutine(startY, endY, card.rectTransform, beatLength));
            yield return new WaitForSeconds(brakeLength);
        }
    }

    public void HideCards()
    {
        for (int i = 0; i < moveCards.Count ; i++)
        {
            UI_MoveCard card = moveCards[i];
            card.SetIcon(currentLevel.value.playerMoves[i+1].icon);
            StartCoroutine(LerpYCoroutine(endY, startY, card.rectTransform, 0.25f));
        }
    }
    
    private IEnumerator LerpYCoroutine(float from, float to, RectTransform rectTransform, float duration)
    {
        float time = 0;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            float newY = Mathf.Lerp(from, to, time / duration);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
            yield return null;
        }

        // Ensure the final position is set exactly to the target position
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, to);
    }
}
