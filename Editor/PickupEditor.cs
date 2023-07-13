using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pickup))]
public class PickupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Pickup p = (Pickup)target;
        if (GUILayout.Button("Activate"))
        {
            GameObject.Find("VO_Manager").GetComponent<VOManager>().Awake();
            p.playMessage();
        }
        if (GUILayout.Button("Stop"))
        {
            p.endMessage();
        }
    }
}
