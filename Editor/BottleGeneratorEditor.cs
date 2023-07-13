//using UnityEngine;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine.SceneManagement;

//[CustomEditor(typeof(GenerateBottleVOs))]
//public class BottleGeneratorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        GenerateBottleVOs script = (GenerateBottleVOs)target;

//        if (GUILayout.Button("Generate"))
//        {
//            script.generate();
//        }

//        //if (GUI.changed)
//        //{
//        //    //EditorUtility.SetDirty(managerScript);
//        //    //EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
//        //    //indexPickups(managerScript);
//        //    script.generate();
//        //    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
//        //}
//    }
//}
