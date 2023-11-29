using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDancer : DancingEntity
{
    [SerializeField] private Move defaultMove;
    [SerializeField] private IntVariable playerActiveMove;
    [SerializeField] private bool isTesting;
    
    #region Tester Vars and Start Function

    private List<int> moveIndexes; // From TesterDancer
    private int currentMoveCount = 0; // From TesterDancer
    protected override void InitializeMoves()
    {
        moveIndexes = currentLevel.moveIndexesForPerfect;
        activeMoveIndex = moveIndexes[currentMoveCount] - 1;
        playerActiveMove.value = activeMoveIndex;
    }
    

    #endregion

    private void Update()
    {
        if (isTesting) return; // Use PlayerDancer's logic when not in testing mode
        
        if (IsAnyOfKeysHeld())
        {
            for (int i = 1; i <= 4; i++) // Iterate over the numbers 1 to 4
            {
                if (Input.GetKeyDown(i.ToString())) // If one of the number keys is pressed
                {
                    activeMoveIndex = i - 1;
                    return; // Exit the method early as we've found the key
                }
            }
        }
        else
        {
            activeMoveIndex = -1;
        }

        playerActiveMove.value = activeMoveIndex;
    }

    private bool IsAnyOfKeysHeld()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKey(i.ToString())) // Check if the key is held down
                return true;
        }

        return false;
    }

    protected override void Move()
    {
        var currentMove = activeMoveIndex == -1 ? defaultMove : currentLevel.playerMoves[activeMoveIndex];
        StartCoroutine(GetMoving(currentMove));
    }

    protected override void UpdateMoveIndex()
    {
        if (!isTesting) // Player logic
        {
            if (activeMoveIndex != -1) activeMoveIndex = -1;
            else events.OnPlayerMissedBeat.Raise();
        }

        else // Testing logic
        {
            currentMoveCount += 1;
            if (currentMoveCount >= moveIndexes.Count) return;
            activeMoveIndex = moveIndexes[currentMoveCount] - 1;
            playerActiveMove.value = activeMoveIndex;
        }
    }

    protected override bool ShouldIStop(Collider2D[] objectsUnderMe)
    {
        if (ShouldIFall(objectsUnderMe.Length))
        {
            Fall();
            return true;
        }

        if (ShouldIWin(objectsUnderMe))
        {
            events.OnWinLevel.Raise();
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
        events.ResetLevel.Raise();
    }
}