using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DanceFloorGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // Assign your tile prefab in the inspector
    public Transform parentObject; // Assign the parent object in the inspector
    public int tilesPerFrame = 10; // Number of tiles to instantiate per frame

    public enum Patterns
    {
        Checkered,
        Spiral
    }

    [EnumToggleButtons] public Patterns pattern; // Choose the pattern from the Inspector

    [Button]
    public void Generate()
    {
        StartCoroutine(GenerateDanceFloor());
    }

    private IEnumerator GenerateDanceFloor()
    {
        switch (pattern)
        {
            case Patterns.Checkered:
                yield return StartCoroutine(GenerateCheckeredPattern());
                break;
            case Patterns.Spiral:
                yield return StartCoroutine(GenerateSpiralPattern());
                break;
        }
    }

    private IEnumerator GenerateCheckeredPattern()
    {
        int count = 0;
        int startingColorIndexForRow = 0; // Initialize the starting color index for the row

        // Access the DanceFloorManager component from the parentObject
        DanceFloorManager danceFloorManager = parentObject.GetComponentInParent<DanceFloorManager>();
        if (danceFloorManager == null)
        {
            Debug.LogError("DanceFloorManager component not found on parentObject.");
            yield break; // Stop the coroutine if DanceFloorManager is not found
        }
        
        danceFloorManager.tiles.Clear();
        
        // Assuming you want each tile to be 1 unit in size
        for (float y = 4.5f; y >= -4.5f; y--)
        {
            int colorIndex = startingColorIndexForRow; // Set the starting color index for the current row

            for (float x = -9.5f; x <= 9.5f; x++)
            {
                // Instantiate the tile prefab at the current position
                Vector2 position = new Vector2(x, y);
                GameObject newTile = Tools.InstantiatePrefab(tilePrefab, position);

                // Set the instantiated tile's parent
                newTile.transform.SetParent(parentObject, false);

                // Access the CustomTile script and set the StartingColorIndex
                CustomTile customTile = newTile.GetComponent<CustomTile>();
                if (customTile != null)
                {
                    customTile.startingColorIndex = colorIndex;
                    customTile.ChangeDefaultColor();
                }

                // Update the color index for the next tile
                colorIndex = (colorIndex + 1) % 4;

                // Add the tile to DanceFloorManager's tiles list
                danceFloorManager.tiles.Add(customTile);

                // Increment the counter
                count++;

                // If we've instantiated the specified number of tiles, wait until the next frame
                if (count >= tilesPerFrame)
                {
                    count = 0; // Reset the counter
                    yield return null; // Wait for the next frame
                }
            }

            // Update the starting color index for the next row
            startingColorIndexForRow = (startingColorIndexForRow + 1) % 4;
        }
    }

    private IEnumerator GenerateSpiralPattern()
    {
        int width = 20; // Total columns
        int height = 10; // Total rows

        // Spiral traversal directions
        Vector2Int[] directions =
        {
            new Vector2Int(1, 0), // Right
            new Vector2Int(0, 1), // Down
            new Vector2Int(-1, 0), // Left
            new Vector2Int(0, -1) // Up
        };

        // Starting conditions
        Vector2Int position = new Vector2Int(-width / 2, height / 2); // Start from the top left corner
        int steps = 1; // Steps to take in the current direction
        int directionIndex = 0; // Index of the current direction
        int colorIndex = 0; // Current color index

        // Control variables
        bool increaseSteps = false; // Whether to increase the step count after a direction change

        for (int i = 0; i < width * height; i++)
        {
            // Instantiate the tile prefab at the current position
            Vector2 worldPosition = new Vector2(position.x + 0.5f, position.y - 0.5f); // Centered position
            GameObject newTile = Tools.InstantiatePrefab(tilePrefab, worldPosition);
            newTile.transform.SetParent(parentObject, false);

            // Set the color index of the tile
            CustomTile customTile = newTile.GetComponent<CustomTile>();
            if (customTile != null)
            {
                customTile.startingColorIndex = colorIndex;
                customTile.ChangeDefaultColor();
            }

            // Update the color index for the next tile
            colorIndex = (colorIndex + 1) % 4;

            // Move to the next position
            position += directions[directionIndex] * steps;

            // Change direction
            directionIndex = (directionIndex + 1) % 4;
            if (increaseSteps) steps++; // Increase steps every second turn
            increaseSteps = !increaseSteps; // Flip the flag every turn

            if ((i + 1) % tilesPerFrame == 0)
            {
                yield return null; // Wait for the next frame
            }
        }
    }

    [Button]
    public void CheckerExistingTiles()
    {
        CustomTile[] allTiles = FindObjectsOfType<CustomTile>();
        float minX = allTiles.Min(tile => tile.transform.position.x);
        float minY = allTiles.Min(tile => tile.transform.position.y);

        foreach (CustomTile tile in allTiles)
        {
            int relativeX = Mathf.FloorToInt(tile.transform.position.x - minX);
            int relativeY = Mathf.FloorToInt(tile.transform.position.y - minY);

            int colorIndex = (relativeX + relativeY) % 4;
            tile.startingColorIndex = colorIndex;
            tile.ChangeDefaultColor();
        }
    }
}