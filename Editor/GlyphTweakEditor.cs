using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweakGlyphs))]
public class GlyphTweakEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TweakGlyphs script = (TweakGlyphs)target;

        if (GUILayout.Button("Select"))
        {
            script.selectGlyphGroup();
        }
    }



}
