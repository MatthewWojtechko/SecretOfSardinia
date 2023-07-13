//using UnityEngine;
//using UnityEditor;
//using System;

//[CustomEditor(typeof(GuidanceManager))]
//public class GuidanceManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        GuidanceManager g = (GuidanceManager)target;

//        GUILayout.Label("Debug");
//        int input = EditorGUILayout.IntField(0);

//        if (GUILayout.Button("Display"))
//            g.playGuidance(input);
//        if (GUILayout.Button("Disappear"))
//            g.closeGuidance();

//    }
//}
