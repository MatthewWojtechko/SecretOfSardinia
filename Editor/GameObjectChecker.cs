using UnityEditor;
using UnityEngine;

public class GameObjectChecker : EditorWindow
{
    string watingMessage = "Select a Game Obejct to find its distance from the camera.";
    GameObject selectedGO;

    [MenuItem("Custom Utilities/Game Object Checker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GameObjectChecker));
    }

    //private void OnEnable()
    //{
    //    Selection.selectionChanged += OnUpdate;
    //}
    //private void OnDisable()
    //{
    //    Selection.selectionChanged -= OnUpdate;
    //}


    private void OnSelectionChange()
    {
        Repaint();
    }
    private void OnGUI()
    {
        selectedGO = Selection.activeObject as GameObject;
        if (selectedGO == null)
            EditorGUILayout.LabelField(watingMessage, EditorStyles.boldLabel);
        else
        {
            Vector3 cameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
            float distance = Vector3.Distance(cameraPosition, selectedGO.transform.position);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Distance of [" + selectedGO.name + "]: ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("" + distance);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Scene: " + selectedGO.scene.name);
        }
    }
}
