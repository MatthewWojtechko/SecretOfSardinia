using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveGroup : EditorWindow
{
    public Vector3 movement;
    List<Vector3> startPositions;

    public void Awake()
    {
        Selection.selectionChanged += updateStartPositions;
    }
    public void OnDestroy()
    {
        Selection.selectionChanged -= updateStartPositions;
    }

    [MenuItem("Custom Utilities/Group Mover")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MoveGroup));
    }

    public void OnGUI()
    {
        if (startPositions == null || startPositions.Count == 0)
        {
            EditorGUILayout.LabelField("Select some game objects to move them all!");
        }
        else
        {
            movement = EditorGUILayout.Vector3Field("Translate by", movement);
            // Move all the Selected gameobjects
            for (int i = 0; i < startPositions.Count; i++)
            {
                Selection.gameObjects[i].transform.position = startPositions[i] + movement;
            }
        }
    }

    void updateStartPositions()
    {
        startPositions = new List<Vector3>();
        foreach (GameObject go in Selection.gameObjects)
        {
            startPositions.Add(go.transform.position);
        }
        movement = Vector3.zero;
    }
}
