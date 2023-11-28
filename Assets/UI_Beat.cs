using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Beat : MonoBehaviour
{
    private Level currentLevel;
    [SerializeField] private int everyXBeats = 2;
    private float beatInterval;
    [SerializeField] private Image image;

    public void Initialize(Component sender, object data)
    {
        currentLevel = (Level)data;
        beatInterval = Tools.GetIntervalLengthFromBPM(currentLevel.defaultSong, everyXBeats);
    }

    public void OnMarker(Component sender, object data)
    {
    }

    public void OnBeat(Component sender, object data)
    {
        int beatNum = (int)data;
        if (beatNum % 2 != 0)
        {
            StartCoroutine(FillOverTime(beatInterval));
        }
    }

    private IEnumerator FillOverTime(float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(0, 1, timeElapsed / duration);

            // Update your fill here
            // For example, if you're filling an UI image, you can do:
            // yourImage.fillAmount = fillAmount;

            yield return null;
        }

        // Ensure the fill is complete in case of any discrepancies
        // yourImage.fillAmount = 1.0f;
    }
}