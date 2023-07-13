//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using UnityEditor.SceneManagement;
//using UnityEngine.SceneManagement;

//[CustomEditor(typeof(FragPickupManager))]
//public class FragPickupManagerEditor : Editor
//{
//    int ID = -1;
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        if (Application.isEditor)   // Running the below functions during playmode causes errors. Errors persist into edit mode, seemingly because the accessed file stays opened, so the DataLoader cannot access it. My solution was to follow the path and delete the files, creating new ones, and adding this if-statement to avoid the problem in the future.
//        {
//            ID = EditorGUILayout.IntField("Status to edit: ", ID);

//            if (GUILayout.Button("Find FRAGMENT"))
//                FragPickupManager.modifyFrag(ID, true);
//            if (GUILayout.Button("Lose FRAGMENT"))
//                FragPickupManager.modifyFrag(ID, false);

//            if (GUILayout.Button("Learn GLYPH"))
//                FragPickupManager.modifyGlyph(ID, true);
//            if (GUILayout.Button("Forget GLYPH"))
//                FragPickupManager.modifyGlyph(ID, false);

//            if (GUILayout.Button("View"))
//            {
//                FragPickupManager.debugSetup();
//                FragPickupManager.debugMsg();
//            }

//            if (GUILayout.Button("Reset All Data"))
//                FragPickupManager.resetData();
//        }
//    }
//}
