using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RemoveDuplicateComponents : EditorWindow
{
    [MenuItem("Custom Utilities/Remove Duplicate Components")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RemoveDuplicateComponents));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Remove Duplicate Components"))
        {
            RemoveDuplicates();
        }
    }

    void RemoveDuplicates()
    {
        // Get selected gameobjects
        GameObject[] selectedGameObjects = Selection.gameObjects;

        // Loop through selected gameobjects
        foreach (GameObject gameObject in selectedGameObjects)
        {
            // Create a list to store unique components
            List<Component> uniqueComponents = new List<Component>();

            // Loop through components in the gameobject
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                // Loop through all unique components
                foreach (Component C in uniqueComponents)
                {
                    if (component.GetType().Equals(C.GetType()))
                    {
                        // Destroy the duplicate component
                        DestroyImmediate(component);
                        continue;
                    }
                }
                uniqueComponents.Add(component);
            }
        }
    }
}

// Written by ChatGPT from the following prompt:
// "Write an editor script that removes duplicate components from the selected gameobjects in Unity."

// I made it correct its answer by saying:
// "Fix your code. By "duplicate," I mean components that are of the same type - not that are the same exact instance. Make your code remove duplicate components."

// The code then became a bit needlessly complicated, so I refined the code inside the RemoveDuplicates() function's loop myself.
