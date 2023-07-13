using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomHierarchy
{
    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go != null)
        {
            CustomHierachyVisual customDisplay = go.GetComponent<CustomHierachyVisual>();

            if (customDisplay != null)
            {
                if (customDisplay.displayVisual())
                {
                    GUI.Label(new Rect(selectionRect.x + selectionRect.width - 20, selectionRect.y, 20, 20), EditorGUIUtility.IconContent(customDisplay.icon));
                }
            }
        }
    }
}

