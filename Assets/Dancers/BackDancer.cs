using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDancer : MonoBehaviour
{
    private int currentMoveIndex;
    
    [SerializeField] private List<Move> moves;
    [SerializeField] private float moveBreakPeriodMultiplier;
    private SpriteRenderer sr;
    private Animator anim;
    private bool canMove;
    private float beatInterval;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private Song song;
    [SerializeField] private int everyXBeats;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        beatInterval = Tools.GetIntervalLengthFromBPM(song, everyXBeats);
        anim.speed = 1 / beatInterval;
        canMove = true;
    }

    public void OnMarker(Component sender, object data)
    {
        string markerString = (string)data;
        if (markerString == "1")
        {
            canMove = true;
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
        StartCoroutine(GetMoving(moves[currentMoveIndex]));
        currentMoveIndex += 1;
        if (currentMoveIndex >= moves.Count)
        {
            currentMoveIndex = 0;
        }
    }

    private IEnumerator GetMoving(Move move)
    {
        float moveBreakDuration = move.ignoreBreakBetweenSteps ? 0 : beatInterval * moveBreakPeriodMultiplier;
        float stepDuration = (beatInterval / move.steps.Count) - moveBreakDuration;
        float animSpeed = 1 / stepDuration;
        anim.speed = animSpeed;


        foreach (Step step in move.steps)
        {
            anim.Play(step.animName, -1, 0f);

            Vector3 start = transform.position; // Use Vector3 to keep the z coordinate
            Vector2 direction2D = Tools.GetDirectionVector(step.direction);
            Vector3 direction = new Vector3(direction2D.x, direction2D.y, 0); // Convert to Vector3 with z set to 0
            Vector3 finish = start + direction * step.length; // Calculate the finish position

            float timePast = 0f;

            while (timePast < stepDuration)
            {
                timePast += Time.deltaTime;

                float linearTime = timePast / stepDuration; // Normalized time (0 to 1)
                float heightTime = jumpCurve.Evaluate(linearTime); // Get the curve value for height

                float height = Mathf.Lerp(0f, step.moveHeight, heightTime); // Interpolate the height

                // Create a new Vector3 for the interpolated position, maintaining the original z value
                Vector3 newPosition = Vector3.Lerp(start, new Vector3(finish.x, finish.y, start.z), linearTime);
                newPosition.y += height; // Apply the height offset

                transform.position = newPosition; // Update the position with the z coordinate unchanged

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

        Destroy(gameObject);
    }

    private bool ShouldIFall()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        if (hitColliders.Length > 0)
        {
            return false;
        }

        return true;
    }
}