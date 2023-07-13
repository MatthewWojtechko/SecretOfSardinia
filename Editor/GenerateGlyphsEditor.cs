using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GenerateGlyphs))]
public class GenerateGlyphsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerateGlyphs genScript = (GenerateGlyphs)target;

        GUILayout.Label(genScript.getSettings()?.name);


        DrawDefaultInspector();

        if (GUILayout.Button("Update"))
        {
            genScript.generate(false);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Generate"))
        {
            if (EditorUtility.DisplayDialog("Really delete glyphs for " + genScript.getSettings()?.name + "?", "Woah there! Generating new glyphs will first destroy the existing glyphs for the current story, which cannot be undone. If you want to preserve the layout of these glyphs, use \"Update\" instead.",
                                            "Proceed", "Cancel"))
            {
                genScript.generate(true);
            }
        }

        if (GUILayout.Button("Parent"))
            genScript.parentAllGlyphs();

        if (GUILayout.Button("Unparent"))
            genScript.unparent();

        if (GUILayout.Button("Set Up Arrays"))
        {
            genScript.setupGlyphManager();
            EditorUtility.SetDirty(genScript.glyphManagerScript);
        }

        if (GUILayout.Button("Array Debug"))
            genScript.debugOutput();

        if (GUILayout.Button("Show Word Count"))
            genScript.displayWordCount();

        if (GUILayout.Button("Destroy"))
        {
            if (EditorUtility.DisplayDialog("Really delete glyphs for " + genScript.getSettings()?.name + "?", "Yikes! This cannot be undone.",
                                "Delete!", "Cancel"))
            {
                genScript.degenerate();
            }
        }

        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

    }
}
