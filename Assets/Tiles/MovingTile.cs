using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : CustomTile
{
    private Vector3 firstPosition, secondPosition;
    [SerializeField] private Vector3 moveDirection;
    private Vector3 currentPos;
    
    [Range(1, 4)]
    [SerializeField] private int moveEveryXBeats = 2;
    [SerializeField] private float intervalMult = 0.5f;
    private float beatInterval;

    public void Initialize(Component sender, object data)
    {
        Level currentLevel = (Level)data;
        beatInterval = Tools.GetIntervalLengthFromBPM(currentLevel.defaultSong, intervalMult);
        beatInterval *= 0.5f;
        firstPosition = transform.position;
        secondPosition = firstPosition + moveDirection;
    }

    public void OnBeat(Component sender, object data)
    {
        int beatNum = (int)data;
        if (beatNum % moveEveryXBeats == 0)
        {
            StartCoroutine(MoveOverTime());
        }
    }
    
    private IEnumerator MoveOverTime()
    {
        float timeElapsed = 0;
        bool isInFirstPos = transform.position == firstPosition;
        Vector3 startPosition = isInFirstPos ? firstPosition : secondPosition;
        Vector3 endPosition = isInFirstPos ? secondPosition : firstPosition;

        // Calculate the actual duration for the movement
        float actualMovementDuration = beatInterval - 0.15f;

        while (timeElapsed < actualMovementDuration)
        {
            timeElapsed += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / actualMovementDuration);
            transform.position = newPosition;
            yield return null;
        }

        // Ensure the object is exactly at the end position
        transform.position = endPosition;

        // Optional: Wait for the remaining time of the beat interval
        yield return new WaitForSeconds(beatInterval - timeElapsed);
    }
}