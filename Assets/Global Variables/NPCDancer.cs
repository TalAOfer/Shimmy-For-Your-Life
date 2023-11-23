using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCDancer : DancingEntity
{
    [SerializeField] private AllMoves allMoves;
    private List<Move> moves;

    public override void InitializeMoves(Level level)
    {
        moves = new List<Move>();
        int numOfMoves = Random.Range(3, 5);
        List<int> randoms = Tools.GetRandoms(numOfMoves, 0, allMoves.moves.Count - 1);
        for (int i = 0; i < randoms.Count; i++)
        {
            moves.Add(allMoves.moves[randoms[i]]);
        }
    }
    
    

    public override void Move()
    {
        StartCoroutine(GetMoving(moves[activeMoveIndex]));
    }

    public override void UpdateMoveIndex()
    {
        activeMoveIndex += 1;
        if (activeMoveIndex >= moves.Count)
        {
            activeMoveIndex = 0;
        }
    }

    public override bool ShouldIStop(Collider2D[] objectsUnderMe)
    {
        if (ShouldIFall(objectsUnderMe.Length))
        {
            if (!is3D.value)
            {
                StartCoroutine(Fall2D());
                return true;
            }

            StartCoroutine(Fall3D(5, 10));
            return true;
        }

        return false;
    }

    public override void OnEndFall()
    {
        Destroy(gameObject);
    }
}