using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveCardManager : MonoBehaviour
{
    [SerializeField] private IntVariable playerActiveMove;
    [SerializeField] private List<UI_MoveCard> moveCards;
    private List<UI_MoveCard> activeCards = new();
    
    private Level currentLevel;
    private float startY;
    private float endY = -70f;

    public void Initilaize(Component sender, object data)
    {
        currentLevel = (Level)data;
        
        startY = moveCards[0].rectTransform.anchoredPosition.y;
        
        InitializeCards();
        StartCoroutine(ShowCards(1f));
    }

    private void InitializeCards()
    {
        for (var i = 0; i < moveCards.Count; i++)
        {
            if (i < currentLevel.playerMoves.Count)
            {
                activeCards.Add(moveCards[i]);
            }
            else
            {
                moveCards[i].gameObject.SetActive(false);
            }
        }
    }
    
    private IEnumerator ShowCards(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        float beatLength = Tools.GetIntervalLengthFromBPM(currentLevel.defaultSong, 1);
        float brakeLength = beatLength / 4f;
        
        for (int i = 0; i < activeCards.Count ; i++)
        {
            UI_MoveCard card = activeCards[i];
            card.SetIcon(currentLevel.playerMoves[i].icon);
            StartCoroutine(LerpYCoroutine(startY, endY, card.rectTransform, beatLength));
            yield return new WaitForSeconds(brakeLength);
        }
    }

    private void Update()
    {
        int activeMove = playerActiveMove.value;
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
    
    public void HideCards()
    {
        for (int i = 0; i < activeCards.Count ; i++)
        {
            UI_MoveCard card = moveCards[i];
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
