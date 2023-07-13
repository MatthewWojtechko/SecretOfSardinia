//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(DialogData))]
//public class DialogDataEditor : Editor
//{
//    private GUIStyle headingStyle = new GUIStyle();
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        DialogData data = (DialogData)target;

//        headingStyle.fontSize = 25;
//        GUILayout.Label("Dialog", headingStyle);//
//        listAllDialog(data);
//    }

//    void listAllDialog(DialogData data)
//    {
//        Subtitle[] messages = data.messages;
//        foreach (Subtitle S in messages)
//        {
//            string output = "[" + S.delay + "] " + S.text;
//            GUILayout.Label(output);//
//        }
//    }
//}
