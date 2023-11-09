using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using UnityEditor;

public class LevelEditor : OdinEditorWindow
{
    [MenuItem("Tools/Level Editor")]
    private static void OpenWindow()
    {
        GetWindow<LevelEditor>().Show();
    }

    public GameObject tilePrefab;
    public Move move;
    public bool withFirst;
    
    [Button("Create Tile")]
    public void CreateTile()
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

        // If you're using Odin and working in the editor, make sure to record the undo so you can undo this action
#if UNITY_EDITOR
    Undo.RegisterCreatedObjectUndo(parentObject, "Create Tile Parent");
#endif
    }
}