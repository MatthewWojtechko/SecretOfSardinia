using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishSoundController))]
public class FishAudioControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FishSoundController c = (FishSoundController)target;

        if (GUILayout.Button("Seen"))
        {
            c.seen();
        }
        else if (GUILayout.Button("Unseen"))
        {
            c.unSeen();
        }
    }
}