using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameplayManager))]
public class GameplayManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameplayManager managerScript = (GameplayManager)target;
        if (GUILayout.Button("Index Pickups"))
        {
            managerScript.indexPickups();
        }

        // Make changes caused by editor persist into gameplay
        if (GUI.changed)
        {
            //EditorUtility.SetDirty(managerScript);
            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            indexPickups(managerScript);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
    void indexPickups(GameplayManager gameMan)
    {
        int index = 0;
        gameMan.secrets = new bool[gameMan.pickups.childCount];
        EditorUtility.SetDirty(gameMan);
        foreach (Transform child in gameMan.pickups)
        {
            child.transform.GetChild(0).GetComponent<VesselSaveLoad>().id = index;
            EditorUtility.SetDirty(child.transform.GetChild(0).GetComponent<Pickup>());
            index++;
        }
    }
}