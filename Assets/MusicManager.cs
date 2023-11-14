using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private CurrentLevel currentLevel;
    private FMOD.Studio.EventInstance musicInstance;

    private TimelineInfo timelineInfo = null;
    private GCHandle timelineHandle;

    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    [SerializeField] private GameEvent OnBeat, OnMarker;
    
    
    private int lastBeat = 0;
    private string lastMarkerString = null;
    
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }
    
    private void Awake()
    {
        if (!currentLevel.value.defaultSong.musicEvent.IsNull) 
        {
            musicInstance = RuntimeManager.CreateInstance(currentLevel.value.defaultSong.musicEvent);
            musicInstance.start();
        }
    }

    private void Start()
    {
        if (!currentLevel.value.defaultSong.musicEvent.IsNull)
        {
            timelineInfo = new TimelineInfo();
            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
            musicInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
            musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        }
    }

    public void Update()
    {
        if (lastMarkerString != timelineInfo.lastMarker)
        {
            lastMarkerString = timelineInfo.lastMarker;
            OnMarker.Raise(this, lastMarkerString);
        }

        if (lastBeat != timelineInfo.currentBeat)
        {
            lastBeat = timelineInfo.currentBeat;
            OnBeat.Raise(this, lastBeat);
        }
    }
    private void OnDestroy()
    {
        
        if (!currentLevel.value.defaultSong.musicEvent.IsNull)
        {
            // Remove the callback
            musicInstance.setCallback(null);

            // Stop the music instance
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            // Release the music instance
            musicInstance.release();

            // Check if the GCHandle is allocated before freeing it
            if (timelineHandle.IsAllocated)
            {
                timelineHandle.Free();
            }
        }
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if(result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;
    
            switch(type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    timelineInfo.currentBeat = parameter.beat;
                }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.lastMarker = parameter.name;
                }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }
}