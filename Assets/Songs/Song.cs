using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Song")]
public class Song : ScriptableObject
{
    public FMODUnity.EventReference musicEvent;
    public float bpm;
}
