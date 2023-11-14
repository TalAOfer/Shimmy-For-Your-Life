using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatUI : MonoBehaviour
{

    public AnimationCurve beatCurve; // Set this up in the inspector for a nice beating effect
    public float onBeatScale = 1.1f; // Multiplier for the on-beat
    private Animator anim;
    [SerializeField] private Song song;

    private Vector3 originalScale;
    private float beatInterval;

    private void Awake()
    {
        originalScale = transform.localScale;
        anim = GetComponent<Animator>();
        
        beatInterval = Tools.GetIntervalLengthFromBPM(song, 1);
        anim.speed = 1 / beatInterval;
        anim.Play("Heart_Beat");
    }

    public void OnBeat()
    {
        anim.Play("Heart_Beat", -1, 0f);

        //StartCoroutine(HeartbeatEffectCoroutine(Vector3.one * 1.1f, beatInterval));
    }

    private IEnumerator HeartbeatEffectCoroutine(Vector3 targetScale, float duration)
    {
        float elapsedTime = 0;
        anim.Play("Heart_Beat", -1, 0f);
        
        // Set initial scale to targetScale at the beginning
    
        while (elapsedTime < duration)
        {
            float t = beatCurve.Evaluate(elapsedTime / duration);
            // Lerp from targetScale to originalScale instead
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it returns to the original scale
        transform.localScale = originalScale;
    }
}
