// Built following this tutorial: https://www.youtube.com/watch?v=-GyvY-kq35M

using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabGeneratorWindow : EditorWindow
{
    private string pathName = "Prefabs/";

    [MenuItem("Custom Utilities/Mass Prefab Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabGeneratorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Mass Prefab Generator", EditorStyles.boldLabel);
        pathName = EditorGUILayout.TextField("Save Path", pathName);

        if (GUILayout.Button("Build Prefabs"))
            GeneratePrefabs();
    }

    private void GeneratePrefabs()
    {
        if (Directory.Exists($"Assets/{pathName}"))
            Directory.CreateDirectory($"Assets/{pathName}");

        foreach (GameObject GO in Selection.gameObjects)
        {
            string localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{pathName}{GO.name}.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(GO, localPath, InteractionMode.UserAction);
        }
    }
}
