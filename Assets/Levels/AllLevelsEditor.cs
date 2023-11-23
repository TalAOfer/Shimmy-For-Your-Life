#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(AllLevels))]
public class AllLevelsEditor : OdinEditor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector options
        base.OnInspectorGUI();

        // Casting the target object to AllLevels
        AllLevels allLevels = (AllLevels)target;

        // Check if the levels list is not null
        if (allLevels.levels != null)
        {
            // Iterate over all levels
            foreach (Level level in allLevels.levels)
            {
                GUILayout.BeginHorizontal();

                // Display the level's name or any other property
                EditorGUILayout.LabelField(level.sceneName);

                // Button to go to the scene
                if (GUILayout.Button("Go To Scene", GUILayout.Width(100)))
                {
                    level.GotoScene();
                }

                GUILayout.EndHorizontal();
            }
        }

        // Apply changes to the serialized object, if any
        if (GUI.changed)
        {
            EditorUtility.SetDirty(allLevels);
        }
    }
}

#endif
