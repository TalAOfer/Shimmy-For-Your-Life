using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

public class Toolkit : OdinEditorWindow
{
    [MenuItem("Tools/Toolkit")]
    private static void OpenWindow()
    {
        GetWindow<Toolkit>().Show();
    }
    
    [TabGroup("Level Picker")]
    public AllLevels allLevels; // Drag your AllLevels ScriptableObject here in the Inspector.
    [TabGroup("Level Picker")]
    public CurrentLevel currentLevel;
    
    // This custom drawer method is specifically for the 'Level Picker' tab.
    [TabGroup("Level Picker"), OnInspectorGUI]
    private void DrawLevelPickerTab()
    {
        if (allLevels != null && allLevels.levels != null)
        {
            // Define the number of buttons per row based on the current window width
            int buttonsPerRow = Mathf.Max(1, (int)(EditorGUIUtility.currentViewWidth) / 150);
            int currentButtonCount = 0;

            GUILayout.BeginHorizontal(); // Start the horizontal layout

            foreach (Level level in allLevels.levels)
            {
                if (level != null)
                {
                    // Increment the current button count
                    currentButtonCount++;

                    // If we've reached the maximum number of buttons per row, wrap to the next line
                    if (currentButtonCount > buttonsPerRow)
                    {
                        GUILayout.EndHorizontal(); // End the current horizontal layout
                        GUILayout.BeginHorizontal(); // Begin a new horizontal layout
                        currentButtonCount = 1; // Reset the button count for the new row
                    }

                    // Draw the button
                    if (GUILayout.Button(level.sceneName, GUILayout.Width(75), GUILayout.Height(75)))
                    {
                        currentLevel.value = level;
                        level.GotoScene(); // Call the GoToScene method when the button is clicked
                    }
                }
            }

            GUILayout.EndHorizontal(); // End the last horizontal layout
        }
    }
    
    [TabGroup("Tile Maker")]
    public GameObject tilePrefab;

    [TabGroup("Tile Maker")]
    public Move move;

    [TabGroup("Tile Maker")]
    public bool withFirst;

    [TabGroup("Tile Maker")]
    [Button("Create Tile")]
    private void CreateTile()
    {
        // Create the parent GameObject
        GameObject parentObject = new GameObject(move.name);
        parentObject.GetOrAddComponent<SnapToGrid>();

        // This will hold the current position while we iterate through the steps
        Vector3 currentPosition = Vector3.zero;

        // Instantiate the start tile at the origin (0,0)
        GameObject startTile = Instantiate(tilePrefab, currentPosition, Quaternion.identity);
        startTile.transform.SetParent(parentObject.transform);

        // Loop through each step and instantiate the tiles
        for (int i = 0; i < move.steps.Count; i++)
        {
            var step = move.steps[i];

            // Skip the first step if withFirst is false
            if (!withFirst && i == 0) continue;

            // Determine the direction vector for the current step
            Vector2 stepDirection = Tools.GetDirectionVector(step.direction);

            // For each unit in step length, instantiate a tile
            for (int j = 0; j < step.length; j++)
            {
                // Move the currentPosition in the direction of the step
                currentPosition += new Vector3(stepDirection.x, stepDirection.y, 0);

                // Instantiate the tile at the current position
                GameObject newTile = Instantiate(tilePrefab, currentPosition, Quaternion.identity);
                newTile.transform.SetParent(parentObject.transform);
            }
        }

        Undo.RegisterCreatedObjectUndo(parentObject, "Create Tile Parent");
    }
    
}