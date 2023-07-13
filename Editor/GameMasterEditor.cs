using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameMaster))]
public class GameMasterEditor : Editor
{
    int sceneToOpen = -1;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameMaster script = (GameMaster)target;

        sceneToOpen = EditorGUILayout.IntField("Scene to Open: ", sceneToOpen);

        if (GUILayout.Button("Open Scene"))
            script.open((GameMaster.GameScreen)sceneToOpen, false, Color.white);
    }
}
