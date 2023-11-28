using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterDancer : DancingEntity
{
    public GameEvent ResetLevel;
    public GameEvent WinLevel;
    public GameEvent OnPlayerMissedBeat;
    [SerializeField] private Move defaultMove;
    [SerializeField] private IntVariable playerActiveMove;
    [SerializeField] private List<int> moveIndexes;
    private int currentMoveCount = 0;

    private void Start()
    {
        activeMoveIndex = moveIndexes[currentMoveCount] - 1;
        playerActiveMove.value = activeMoveIndex;
    }

    public override void Move()
    {
        Move currentMove = activeMoveIndex == -1 ? defaultMove : currentLevel.playerMoves[activeMoveIndex];
        StartCoroutine(GetMoving(currentMove));
    }

    public override void UpdateMoveIndex()
    {
        currentMoveCount += 1;
        if (currentMoveCount >= moveIndexes.Count) return;
        activeMoveIndex = moveIndexes[currentMoveCount] - 1;
        playerActiveMove.value = activeMoveIndex;
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

        if (ShouldIWin(objectsUnderMe))
        {
            WinLevel.Raise();
            anim.speed = 1;
            anim.Play("Win");
            SetMovement(false);
            return true;
        }

        return false;
    }

    private bool ShouldIWin(Collider2D[] objectsUnderMe)
    {
        foreach (var coll in objectsUnderMe)
        {
            if (coll.CompareTag("Flag"))
            {
                return true;
            }
        }

        return false;
    }

    public override void OnEndFall()
    {
        ResetLevel.Raise();
    }
}