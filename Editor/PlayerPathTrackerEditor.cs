using UnityEditor;
using UnityEngine;
using BrokenVector.PersistentComponents;

[CustomEditor(typeof(PlayerPathTracker))]
public class PlayerPathTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        PlayerPathTracker script = (PlayerPathTracker)target;

        if (GUILayout.Button("Begin"))
        {
            PersistentComponents.Instance.WatchComponent(script);
            script.begin();
        }
        if (GUILayout.Button("End"))
        {
            script.end();
        }
        if (GUILayout.Button("Apply"))
        {
            script.instantiateGameobjectsAtPoints();
        }
    }
}