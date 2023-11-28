using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDancer : DancingEntity
{
    public GameEvent ResetLevel;
    public GameEvent WinLevel;
    public GameEvent OnPlayerMissedBeat;
    [SerializeField] private Move defaultMove;
    [SerializeField] private IntVariable playerActiveMove;

    void Update()
    {
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

    bool IsAnyOfKeysHeld()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKey(i.ToString())) // Check if the key is held down
                return true;
        }

        return false;
    }

    public override void Move()
    {
        Move currentMove = activeMoveIndex == -1 ? defaultMove : currentLevel.playerMoves[activeMoveIndex];
        StartCoroutine(GetMoving(currentMove));
    }

    public override void UpdateMoveIndex()
    {
        if (activeMoveIndex != -1) activeMoveIndex = -1;
        else OnPlayerMissedBeat.Raise();
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