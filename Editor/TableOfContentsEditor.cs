using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VesselText;
using UnityEditor;

[CustomEditor(typeof(TableOfContents))]
public class TableOfContentsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TableOfContents script = (TableOfContents)target;
        if (GUILayout.Button("Organize Chapters"))
        {
            script.organizeMonologues();
            EditorUtility.SetDirty(script);
        }
        if (GUILayout.Button("Set ID"))
        {
            script.setMonologueIDs();
        }

        //// Debug
        //string msg = "Chapter Array:\n";
        //if (script.Monologues_ChapterOrganized != null)
        //{
        //    for (int i = 0; i < script?.Monologues_ChapterOrganized.Length; i++)
        //    {
        //        msg += i + ")   ";
        //        for (int j = 0; j < script.Monologues_ChapterOrganized[i].Length; j++)
        //        {
        //            msg += script.Monologues_ChapterOrganized[i][j].title + "   ";
        //        }
        //        msg += "\n";
        //    }
        //}
        //EditorGUILayout.TextArea(msg);
    }
}
