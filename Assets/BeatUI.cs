using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatUI : MonoBehaviour
{

    public AnimationCurve beatCurve; // Set this up in the inspector for a nice beating effect
    public float onBeatScale = 1.3f; // Multiplier for the on-beat
    public float offBeatScale = 1.1f; // Multiplier for the off-beat
    private Animator anim;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    public void Initialize(float beatDuration)
    {
        anim.speed = 1 / beatDuration;
        anim.Play("Heart_Beat");
    }

    public void OnBeat()
    {
        StartCoroutine(HeartbeatEffectCoroutine(Vector3.one * 1.1f, 1));
    }

    private IEnumerator HeartbeatEffectCoroutine(Vector3 targetScale, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = beatCurve.Evaluate(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it returns to the original scale
        transform.localScale = originalScale;
    }
}
