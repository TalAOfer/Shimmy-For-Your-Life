using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TileColors colors;
    [Range(0, 3)]
    [SerializeField] public int startingColorIndex = 0;
    private int currentColorIndex;
    [SerializeField] private bool shouldFall;
    [SerializeField] private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        currentColorIndex = startingColorIndex;
        if (shouldFall) ApplyFall();
    }

    public void ApplyFall()
    {
        StartCoroutine(Fall());
    }
    private IEnumerator Fall()
    {
        coll.enabled = false;
        
        float targetZ = 10f;
        float lerpSpeed = 3f; // Adjust the lerp speed to your preference
        
        // Continue the loop until the localPosition.z is approximately equal to the targetZ
        while (transform.localPosition.z < targetZ - 0.05f)
        {
            // Calculate the new position
            Vector3 newPosition = transform.localPosition;
            newPosition.z = Mathf.Lerp(newPosition.z, targetZ, Time.deltaTime * lerpSpeed);

            // Update the transform's position
            transform.localPosition = newPosition;

            // Wait until the next frame
            yield return null;
        }
        
        Destroy(gameObject);
    }
    
    private void OnValidate()
    {
        ChangeDefaultColor(); // Update the color to the new index
    }

    public void NextColor()
    {
        currentColorIndex += 1;
        if (currentColorIndex >= colors.colors.Count)
        {
            currentColorIndex = 0;
        }

        ChangeColor();
    }

    public void ChangeColor()
    {
        sr.color = colors.colors[currentColorIndex];
    }

    public void ChangeDefaultColor()
    {
        sr.color = colors.colors[startingColorIndex];
    }
    
}
