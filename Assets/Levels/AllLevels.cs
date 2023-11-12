using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/All Levels")]
public class AllLevels : ScriptableObject
{
    public List<Level> levels;
}
