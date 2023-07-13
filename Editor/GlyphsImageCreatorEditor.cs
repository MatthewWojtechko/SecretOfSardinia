using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(GlyphsImageCreator))]
public class GlyphsImageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GlyphsImageCreator genScript = (GlyphsImageCreator)target;

        if (GUILayout.Button("Set Values"))
            genScript.calculateSettingsFromGlyphs();
        if (GUILayout.Button("Export"))
            genScript.generate();
    }
}