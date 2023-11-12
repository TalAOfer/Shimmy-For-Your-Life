using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Levels/CurrentLevel")]
public class CurrentLevel : ScriptableObject
{
    public Level value;
}
