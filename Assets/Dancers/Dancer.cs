using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DancerType
{
    Player,
    Tester,
    NPC
}

public class Dancer : MonoBehaviour
{
    [SerializeField] private AllEvents events;
    [SerializeField] private Move defaultMove;
    [SerializeField] private DancerType dancerType;
    [SerializeField] private string startMarkerString;
    [SerializeField] private int everyXBeats = 2;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private IntVariable playerActiveMove;
    private Level _currentLevel;
    public BoolVariable is3D;

    private bool _canMove;
    private float _beatInterval;
    private int _activeMoveIndex;

    [SerializeField] protected Animator anim;
    [SerializeField] private SpriteRenderer sr;
    private bool _isInitialized;

    private List<int> _moveIndexes;
    private int _currentMoveCount = 0;
    private List<Move> _moves;
    [SerializeField] private AllMoves allMoves;

    public void Initialize(Component sender, object data)
    {
        _currentLevel = (Level)data;
        InitializeMoves();
        _beatInterval = Tools.GetIntervalLengthFromBPM(_currentLevel.defaultSong, everyXBeats);
        anim.speed = 1 / _beatInterval;
        _isInitialized = true;
        var objectsUnderMe = GetFloorObjects();
        transform.SetParent(GetCurrentTileTransform(objectsUnderMe));
        
        if (dancerType == DancerType.Tester)
        {
            _moveIndexes = _currentLevel.moveIndexesForPerfect;
            _activeMoveIndex = _moveIndexes[_currentMoveCount] - 1;
            playerActiveMove.value = _activeMoveIndex;
        }
    }
    
    private void InitializeMoves()
    {
        _moves = new List<Move>();
        int numOfMoves = Random.Range(3, 5);
        List<int> randoms = Tools.GetRandoms(numOfMoves, 0, allMoves.moves.Count - 1);
        for (int i = 0; i < randoms.Count; i++)
        {
            _moves.Add(allMoves.moves[randoms[i]]);
        }
    }

    private void Move()
    {
        Move currentMove = _currentLevel.playerMoves[_activeMoveIndex];
        if (dancerType != DancerType.NPC)
        {
            if (_activeMoveIndex == -1) currentMove = defaultMove;
        }
        StartCoroutine(GetMoving(currentMove));
    }

    private void UpdateMoveIndex()
    {
        switch (dancerType)
        {
            case DancerType.Player:
                if (_activeMoveIndex != -1) { _activeMoveIndex = -1;}
                else events.OnPlayerMissedBeat.Raise();
                break;
            case DancerType.Tester:
                _currentMoveCount += 1;
                if (_currentMoveCount >= _moveIndexes.Count) return;
                _activeMoveIndex = _moveIndexes[_currentMoveCount] - 1;
                playerActiveMove.value = _activeMoveIndex;
                break;
            case DancerType.NPC:
                _activeMoveIndex += 1;
                if (_activeMoveIndex >= _moves.Count)
                {
                    _activeMoveIndex = 0;
                }
                break;
        }
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

    private void OnEndFall()
    {
        events.ResetLevel.Raise();
    }

    private void SetMovement(bool enable) => _canMove = enable;

    public void OnMarker(Component sender, object data)
    {
        var markerString = (string)data;
        if (markerString == startMarkerString)
        {
            SetMovement(true);
        }
    }

    public void OnBeat(Component sender, object data)
    {
        var beatNum = (int)data;

        if (_isInitialized && _canMove && beatNum % 2 != 0)
        {
            Move();
        }
    }

    private IEnumerator GetMoving(Move move)
    {
        UpdateMoveIndex();

        float moveBreakDuration = move.ignoreBreakBetweenSteps ? 0 : _beatInterval * (1f / (move.steps.Count * 2));
        float stepDuration = (_beatInterval / move.steps.Count) - moveBreakDuration;
        float animSpeed = 1 / stepDuration;
        anim.speed = animSpeed;

        foreach (Step step in move.steps)
        {
            anim.Play(step.animName, -1, 0f);
            //transform.SetParent(null);

            Vector3 start = transform.position; // Use Vector3 to keep the z coordinate
            Vector2 direction2D = Tools.GetDirectionVector(step.direction);
            Vector3 direction = new Vector3(direction2D.x, direction2D.y, 0); // Convert to Vector3 with z set to 0
            Vector3 finish = start + direction * step.length; // Calculate the finish position

            float timePast = 0f;


            while (timePast < stepDuration)
            {
                timePast += Time.deltaTime;
                if (start != finish)
                {
                    float linearTime = timePast / stepDuration; // Normalized time (0 to 1)
                    float heightTime = jumpCurve.Evaluate(linearTime); // Get the curve value for height

                    float height = Mathf.Lerp(0f, step.moveHeight, heightTime); // Interpolate the height

                    // Create a new Vector3 for the interpolated position, maintaining the original z value
                    Vector3 newPosition = Vector3.Lerp(start, new Vector3(finish.x, finish.y, start.z), linearTime);
                    newPosition.y += height; // Apply the height offset

                    transform.position = newPosition; // Update the position with the z coordinate unchanged
                }

                yield return null;
            }

            Collider2D[] objectsUnderMe = GetFloorObjects();

            if (ShouldIStop(objectsUnderMe)) yield break;

            transform.SetParent(GetCurrentTileTransform(objectsUnderMe), true);

            yield return new WaitForSeconds(moveBreakDuration);
        }
    }

    private Collider2D[] GetFloorObjects()
    {
        return Physics2D.OverlapCircleAll(transform.position, 0.1f);
    }

    private bool ShouldIStop(Collider2D[] objectsUnderMe)
    {
        if (ShouldIFall(objectsUnderMe.Length))
        {
            Fall();
            return true;
        }

        if (dancerType != DancerType.NPC && ShouldIWin(objectsUnderMe))
        {
            events.OnWinLevel.Raise();
            anim.speed = 1;
            anim.Play("Win");
            SetMovement(false);
            return true;
        }

        return false;
    }

    private Transform GetCurrentTileTransform(Collider2D[] objectsUnderMe)
    {
        Transform currentTile = null;

        foreach (Collider2D coll in objectsUnderMe)
        {
            if (coll.CompareTag("Tile"))
            {
                currentTile = coll.transform;
            }
        }

        return currentTile;
    }

    private bool ShouldIFall(int collCount)
    {
        return (collCount <= 0);
    }

    private void Fall()
    {
        if (!is3D.value)
        {
            StartCoroutine(Fall2D());
        }
        else
        {
            StartCoroutine(Fall3D(5, 10));
        }
    }

    private IEnumerator Fall2D()
    {
        _canMove = false;
        while (transform.localScale.x > 0.05f)
        {
            Vector3 newScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
            transform.localScale = newScale;

            yield return null;
        }

        OnEndFall();
    }

    private IEnumerator Fall3D(float fallDistance, float fallSpeed)
    {
        _canMove = false;
        sr.sortingOrder = 0;
        Vector3 startPosition = transform.position; // Store the starting position
        Vector3 endPosition =
            new Vector3(startPosition.x, startPosition.y, startPosition.z + fallDistance); // Calculate the end position
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
}