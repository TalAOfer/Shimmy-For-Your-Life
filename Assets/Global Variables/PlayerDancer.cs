using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDancer : DancingEntity
{
    public GameEvent ResetLevel;
    public GameEvent WinLevel;
    [SerializeField] private IntVariable playerActiveMove;
    
    void Update()
    {
        if (IsAnyOfKeysHeld())
        {
            for (int i = 1; i <= 4; i++) // Iterate over the numbers 1 to 4
            {
                if (Input.GetKeyDown(i.ToString())) // If one of the number keys is pressed
                {
                    activeMoveIndex = i;
                    return; // Exit the method early as we've found the key
                }
            }
        }
        else
        {
            activeMoveIndex = 0;
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
        StartCoroutine(GetMoving(currentLevel.value.playerMoves[activeMoveIndex]));
    }

    public override void UpdateMoveIndex()
    {
        //TODO if activeMove is 0, decrement boogie meter.  
        activeMoveIndex = 0;
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