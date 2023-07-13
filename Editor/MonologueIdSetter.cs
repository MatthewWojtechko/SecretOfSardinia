using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using VesselText;
using UnityEngine;

public class MonologueIdSetter : EditorWindow
{
    public Monologue[] monologues;
    public int startID;

    private SerializedObject serializedObject;
    private SerializedProperty arrayProperty;
    [MenuItem("CustomUtilities/Monologue ID Setter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MonologueIdSetter));
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        arrayProperty = serializedObject.FindProperty("monologues");
    }

    void OnGUI()
    {
        // Draw the array property field
        EditorGUILayout.PropertyField(arrayProperty, true);

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();

        startID = EditorGUILayout.IntField("Start ID", startID);

        if (GUILayout.Button("New IDs"))
        {
            for (int i = 0; i < monologues.Length; i++)
            {
                monologues[i].id = startID + i;
            }
        }
    }
}
