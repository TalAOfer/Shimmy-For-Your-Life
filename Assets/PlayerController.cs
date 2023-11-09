using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Song song;
    [SerializeField] private List<Move> moves;
    [SerializeField] private float moveBreakPeriodMultiplier = 0.25f;
    [SerializeField] private int everyXbeats = 2;
    private int activeMove = -1;
    private bool canMove = false;
    private float beatInterval;

    [SerializeField] Tilemap floorMap;

    [SerializeField] GameEvent ResetLevel;

    #region Animation

    private Animator anim;
    [SerializeField] private AnimationCurve jumpCurve;
    private float defaultAnimSpeed;

    #endregion


    public void Start()
    {
        anim = GetComponent<Animator>();
        beatInterval = (song.bpm / 60f) / everyXbeats;
        defaultAnimSpeed = 1 / beatInterval;
    }

    public void Initialize(float beatInterval)
    {
        this.beatInterval = beatInterval;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    void Update()
    {
        if (IsAnyOfKeysHeld())
        {
            for (int i = 1; i <= 4; i++) // Iterate over the numbers 1 to 4
            {
                if (Input.GetKeyDown(i.ToString())) // If one of the number keys is pressed
                {
                    activeMove = i;
                    return; // Exit the method early as we've found the key
                }
            }
        }
        else
        {
            activeMove = 0;
        }
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

    public void OnMarker(Component sender, object data)
    {
        string markerString = (string)data;
        if (markerString == "1")
        {
            EnableMovement();
        }
    }
    
    public void OnBeat(Component sender, object data)
    {
        int beatNum = (int)data;
        
        if (canMove && beatNum % 2 != 0)
        {
            Move();
        }
    }

    private void Move()
    {
        StartCoroutine(GetMoving(moves[activeMove]));
    }

    private IEnumerator GetMoving(Move move)
    {
        activeMove = 0;

        float moveBreakDuration = move.ignoreBreakBetweenSteps ? 0 : beatInterval * moveBreakPeriodMultiplier;
        float stepDuration = (beatInterval / move.steps.Count) - moveBreakDuration;
        float animSpeed = 1 / stepDuration;
        anim.speed = animSpeed;


        foreach (Step step in move.steps)
        {
            anim.Play(step.animName, -1, 0f);

            Vector2 start = transform.position;
            Vector2 finish = start + (Tools.GetDirectionVector(step.direction) * step.length);

            float timePast = 0f;

            while (timePast < stepDuration)
            {
                timePast += Time.deltaTime;

                float linearTime = timePast / stepDuration; // 0 to 1 time
                float heightTime = jumpCurve.Evaluate(linearTime); // value from curve

                float height = Mathf.Lerp(0f, step.moveHeight, heightTime); // clamped between the max height and 0

                transform.position =
                    Vector2.Lerp(start, finish, linearTime) + new Vector2(0f, height); // adding values on y axis

                yield return null;
            }

            if (ShouldIFall())
            {
                StartCoroutine(Fall());
                yield break;
            }

            yield return new WaitForSeconds(moveBreakDuration);
        }
    }

    public IEnumerator Fall()
    {
        canMove = false;
        while (transform.localScale.x > 0.05f)
        {
            Vector3 newScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
            transform.localScale = newScale;

            yield return null;
        }

        ResetLevel.Raise();
    }

    private bool ShouldIFall()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        if (hitColliders.Length > 0)
        {
            foreach (var coll in hitColliders)
            {
                if (coll.CompareTag("Flag"))
                {
                    Debug.Log("I win!");
                }
            }

            return false;
        }

        return true;
    }
}