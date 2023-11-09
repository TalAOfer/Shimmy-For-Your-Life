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

    // Start is called before the first frame update
    void Start()
    {
        currentColorIndex = startingColorIndex;
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
