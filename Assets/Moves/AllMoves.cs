using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/AllMoves")]
public class AllMoves : ScriptableObject
{
    public List<Move> moves;
}
