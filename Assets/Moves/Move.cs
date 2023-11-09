using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Move")]
public class Move : ScriptableObject
{
    
    public List<Step> steps;
    public bool ignoreBreakBetweenSteps;
}

[Serializable]
public class Step 
{
    public Directions direction;

    [HideIf("direction", Directions.InPlace)]
    public int length = 1;
    public float moveHeight = 0;
    public string animName;
}

