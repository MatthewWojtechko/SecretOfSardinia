using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(VirtualCameraManager))]
public class VirtualCameraManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VirtualCameraManager managerScript = (VirtualCameraManager)target;
        if (GUILayout.Button("Cache VCam Data"))
        {
            managerScript.cacheVcamData();
            EditorUtility.SetDirty(managerScript);
            //EditorUtility.SetDirty(managerScript); ///////
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        //// Make changes caused by editor persist into gameplay
        //if (GUI.changed)
        //{
        //    //EditorUtility.SetDirty(managerScript);
        //    //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        //    managerScript.cacheVcamData();
        //    EditorUtility.SetDirty(managerScript);
        //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        //}
    }
}