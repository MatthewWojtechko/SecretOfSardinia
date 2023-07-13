using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerHealth script = (PlayerHealth)target;

        if (GUILayout.Button("Kill"))
            PlayerHealth.kill();
        if (GUILayout.Button("Update Dissolve"))
            script.updateDissolve();
    }
}
