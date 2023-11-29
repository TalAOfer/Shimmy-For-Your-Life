using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCDancer : DancingEntity
{
    [SerializeField] private AllMoves allMoves;
    private List<Move> moves;

    protected override void InitializeMoves()
    {
        moves = new List<Move>();
        int numOfMoves = Random.Range(3, 5);
        List<int> randoms = Tools.GetRandoms(numOfMoves, 0, allMoves.moves.Count - 1);
        for (int i = 0; i < randoms.Count; i++)
        {
            moves.Add(allMoves.moves[randoms[i]]);
        }
    }

    protected override void Move()
    {
        StartCoroutine(GetMoving(moves[activeMoveIndex]));
    }

    protected override void UpdateMoveIndex()
    {
        activeMoveIndex += 1;
        if (activeMoveIndex >= moves.Count)
        {
            activeMoveIndex = 0;
        }
    }

    protected override bool ShouldIStop(Collider2D[] objectsUnderMe)
    {
        if (ShouldIFall(objectsUnderMe.Length))
        {
            Fall();
            return true;
        }

        return false;
    }

    public override void OnEndFall()
    {
        Destroy(gameObject);
    }
}