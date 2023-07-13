using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoundaryController))]
public class BoundaryControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoundaryController b = (BoundaryController)target;

        if (GUILayout.Button("Set"))
        {
            b.setHeight(b.currentHeight);
        }
    }
}
