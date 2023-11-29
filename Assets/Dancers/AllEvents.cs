using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "AllEvents")]
public class AllEvents : ScriptableObject
{
    [Title("Scene Management Events")] 
    public GameEvent ResetLevel;

    [Title("Player Events")] 
    public GameEvent OnWinLevel;
    public GameEvent OnPlayerMissedBeat;
    
}
