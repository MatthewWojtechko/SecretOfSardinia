using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(TerrainToggler))]
public class TerrainTogglerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainToggler toggleScript = (TerrainToggler)target;
        if (GUILayout.Button("This button does nothing"))
        {
            toggleScript.updateCells();
        }

        if (GUI.changed)
        {
            //EditorUtility.SetDirty(managerScript);
            //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            //indexPickups(managerScript);
            //toggleScript.updateCells();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
