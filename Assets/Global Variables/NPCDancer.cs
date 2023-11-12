using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDancer : DancingEntity
{
    [SerializeField] protected List<Move> moves;
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
