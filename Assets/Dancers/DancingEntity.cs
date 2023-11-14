using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DancingEntity : MonoBehaviour
{
    [SerializeField] private float moveBreakPeriodMultiplier = 0.25f;
    [SerializeField] private int everyXBeats = 2;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] protected CurrentLevel currentLevel;
    public BoolVariable is3D;
    
    private bool canMove = false;
    private float beatInterval;
    protected int activeMoveIndex;
    
    protected Animator anim;
    private SpriteRenderer sr;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        beatInterval = Tools.GetIntervalLengthFromBPM(currentLevel.value.defaultSong, everyXBeats);
        anim.speed = 1 / beatInterval;
    }

    public void SetMovement(bool enable) => canMove = enable;
    
    public void OnMarker(Component sender, object data)
    {
        string markerString = (string)data;
        if (markerString == "1")
        {
            SetMovement(true);
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

    public abstract void Move();

    public abstract void UpdateMoveIndex();
    public IEnumerator GetMoving(Move move)
    {
        UpdateMoveIndex();

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

            Collider2D[] objectsUnderMe = GetFloorObjects();
            
            if (ShouldIStop(objectsUnderMe)) yield break;

            yield return new WaitForSeconds(moveBreakDuration);
        }
    }

    public Collider2D[] GetFloorObjects()
    {
        return Physics2D.OverlapCircleAll(transform.position, 0.1f);
    }

    public abstract bool ShouldIStop(Collider2D[] objectsUnderMe);
    
    public bool ShouldIFall(int collCount)
    {
        return (collCount <= 0);
    }
    
    public IEnumerator Fall2D()
    {
        canMove = false;
        while (transform.localScale.x > 0.05f)
        {
            Vector3 newScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
            transform.localScale = newScale;

            yield return null;
        }
        
        OnEndFall();
    }
    public IEnumerator Fall3D(float fallDistance, float fallSpeed)
    {
        canMove = false;
        sr.sortingOrder = 0;
        Vector3 startPosition = transform.position; // Store the starting position
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + fallDistance); // Calculate the end position
        float threshold = 0.1f; // How close we need to get to endPosition to finish falling

        while (Vector3.Distance(transform.position, endPosition) > threshold)
        {
            // Interpolate the position from current to the end position at the specified speed
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * fallSpeed);

            // If the object is very close to the end position, snap it to the end position
            if (Vector3.Distance(transform.position, endPosition) <= threshold)
            {
                transform.position = endPosition;
            }

            yield return null;
        }
        
        OnEndFall();
    }

    public abstract void OnEndFall();

}

