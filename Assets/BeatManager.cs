using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public int bpm; // Beats per minute
    [SerializeField] private GameEvent OnBeat;
    [SerializeField] private AudioSource musicPlayer;
    
    private double nextEventTime;
    private double beatInterval; // Time interval between beats

    private void Start()
    {
        // Calculate the interval between beats in seconds
        beatInterval = 60.0 / bpm;

        // Set the initial next event time
        nextEventTime = AudioSettings.dspTime + beatInterval;

        // Start the coroutine
        StartCoroutine(BeatRoutine());
    }

    private IEnumerator BeatRoutine()
    {
        while (true)
        {
            double currentTime = AudioSettings.dspTime;

            // Check if the current time has reached the time for the next event
            if (currentTime >= nextEventTime)
            {
                Foo(); // Call your function at the beat interval

                // Schedule the next beat
                nextEventTime += beatInterval;

                // In case of a delay, skip the missed beats
                while (AudioSettings.dspTime > nextEventTime)
                {
                    nextEventTime += beatInterval;
                }
            }

            // Yield until the next frame
            yield return null;
        }
    }

    private void Foo()
    {
        // Your Foo function logic here
        OnBeat.Raise();
        musicPlayer.Play();
    }
}

