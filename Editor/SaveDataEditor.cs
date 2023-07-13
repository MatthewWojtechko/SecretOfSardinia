// Built following this tutorial: https://www.youtube.com/watch?v=-GyvY-kq35M

using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using VesselText;

public class SaveDataEditor : EditorWindow
{
    bool[] isDataChanged = new bool[6];
    Vector2[] scrollView = new Vector2[6];
    string slot = "";

    int tablet, fragment, glyph;
    public TableOfContents Guide;

    [MenuItem("Custom Utilities/Save Data Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SaveDataEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Data Editor", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Slot");
        slot = EditorGUILayout.TextField(slot);//EditorGUILayout.IntSlider(slot, 0, 2);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Load"))
            SaveDataManager.loadAll(slot + "");
        if (GUILayout.Button("Reset"))
            SaveDataManager.resetSaveData(slot + "");

        if (!SaveDataManager.isLoaded)
        {
            GUILayout.Label("Either load or reset the data to get started.");
            return;
        }

        EditorGUILayout.BeginHorizontal();

        dataColumn("Fragments", SaveDataManager.fragments, ref isDataChanged[0],  ref scrollView[0], (int i) => "Fragment " + i);//"Tablet " + i + ", Fragment ");
        dataColumn("Glyphs",    SaveDataManager.glyphs,    ref isDataChanged[1],  ref scrollView[1], (int i) => Cipher.translations[i][0]);
        Monologue[] m = TableOfContents.get().Monologues;
        dataColumn("Vessels",   SaveDataManager.vessels,   ref isDataChanged[2],  ref scrollView[2], (int i) => TableOfContents.get().getChapterName(TableOfContents.get().Monologues[i].episode) + "/" +  m[i].title);
        dataColumn("Barriers",  SaveDataManager.barriers,  ref isDataChanged[3],  ref scrollView[3], (i) => i.ToString());
        dataColumn("Trials",    SaveDataManager.trials,    ref isDataChanged[4],  ref scrollView[4], (i) => i.ToString());
        dataColumn("Spire",     SaveDataManager.spire,     ref isDataChanged[5],  ref scrollView[5], (i) => i.ToString());

        EditorGUILayout.EndHorizontal();

        //fragmentEditor();

        
    }

    void massEdit<T>(string n, T d, ref bool isChanged) where T : SaveData<T>
    {
        if (isChanged)
        {
            GUI.color = Color.red;
            EditorGUILayout.LabelField("Warning: Unsaved changes");
            GUI.color = Color.white;
        }

        // Update all at once
        if (typeof(T) != typeof(SaveDataID))
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Gain All " + n))
            {
                isChanged = true;
                d.gainAll();
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Lose All " + n))
            {
                isChanged = true;
                d.loseAll();
            }
        }
    }

    void saveButton<T>(string n, T d, ref bool isChanged) where T : SaveData<T>
    {
        GUI.backgroundColor = Color.white;
        if (GUILayout.Button("Save " + n))
        {
            d.save();
            isChanged = false;
        }
    }

    public void dataColumn<T>(string title, T data, ref bool isChanged, ref Vector2 scroll, Func<int, string> getName) where T : SaveData<T>
    {
        EditorGUILayout.BeginVertical();
        if (typeof(T) != typeof(SaveDataID))
            scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUI.BeginChangeCheck();

        // Display columns
        if (typeof(T) == typeof(SaveDataList))  // List of booleans
        {
            bool[] items = (data as SaveDataList).items;
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = EditorGUILayout.Toggle(getName(i), items[i]);
            }
        }
        else if (typeof(T) == typeof(SaveData2DList))  // 2D List of booleans
        {
            bool[][] items = (data as SaveData2DList).items;
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Length; j++)
                {
                    items[i][j] = EditorGUILayout.Toggle(getName(i) + j, items[i][j]);
                }
            }
        }
        else if (typeof(T) == typeof(SaveDataID))  // Single int
        {
            int items = (data as SaveDataID).ID;
            (data as SaveDataID).ID = EditorGUILayout.IntField((data as SaveDataID).ID);
        }
        else if (typeof(T) == typeof(SaveDataScores))  // List of floats
        {
            float[] items = (data as SaveDataScores).scores;
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = EditorGUILayout.FloatField(getName(i), items[i]);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Invalid Save Data type");
        }

        // Check for changes
        if (EditorGUI.EndChangeCheck())
        {
            isChanged = true;
        }

        if (typeof(T) != typeof(SaveDataID))
            EditorGUILayout.EndScrollView();

        // Set or Unset all
        massEdit(title, data, ref isChanged);
        // Save Functionality
        saveButton(title, data, ref isChanged);

        EditorGUILayout.EndVertical();
    }

    //void fragmentEditor()
    //{
    //    // Gain/Lose fragments
    //    EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("Tablet");
    //        tablet = EditorGUILayout.IntSlider(tablet, 0, SaveDataManager.fragments.items.Length-1);
    //    EditorGUILayout.EndHorizontal();
    //    EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("Fragment");
    //        fragment = EditorGUILayout.IntSlider(fragment, 0, SaveDataManager.fragments.items[tablet].Length-1);
    //    EditorGUILayout.EndHorizontal();

    //    if (SaveDataManager.fragments.items[tablet][fragment])
    //        EditorGUILayout.LabelField("HAVE");
    //    else
    //        EditorGUILayout.LabelField("DON'T HAVE");


    //    if (GUILayout.Button("Find Fragment"))
    //        SaveDataManager.fragments.gain(tablet, fragment);
    //    if (GUILayout.Button("Lose Fragment"))
    //        SaveDataManager.fragments.set(tablet, fragment, false);
    //    GUI.backgroundColor = Color.green;
    //    if (GUILayout.Button("FIND ALL Fragments"))
    //    {
    //        for (int i = 0; i < SaveDataManager.fragments.items[tablet].Length; i++)
    //            SaveDataManager.fragments.gain(tablet, i);
    //    }
    //    GUI.backgroundColor = Color.red;
    //    if (GUILayout.Button("LOSE ALL Fragments"))
    //    {
    //        for (int i = 0; i < SaveDataManager.fragments.items[tablet].Length; i++)
    //            SaveDataManager.fragments.set(tablet, i, false);
    //    }
    //    GUI.backgroundColor = Color.white;



    //   // Gain/Lost Glyphs
    //   EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("Glyph");
    //        glyph = EditorGUILayout.IntSlider(glyph, 0, SaveDataManager.glyphs.items.Length-1);
    //    EditorGUILayout.EndHorizontal();
    //    EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField(Cipher.translations[glyph][0]);
    //        string path = "Assets/Resources/NewSymbolTextures/Glyph (" + (glyph + 1) + ").png";
    //        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
    //        GUILayout.Box(banner, GUILayout.Width(50), GUILayout.Height(50));
    //    EditorGUILayout.EndHorizontal();

    //    if (SaveDataManager.glyphs.items[glyph])
    //        EditorGUILayout.LabelField("HAVE");
    //    else
    //        EditorGUILayout.LabelField("DON'T HAVE");

    //    if (GUILayout.Button("Learn GLYPH"))
    //        SaveDataManager.glyphs.gain(glyph);
    //    if (GUILayout.Button("Forget GLYPH"))
    //        SaveDataManager.glyphs.set(glyph, false);
    //    GUI.backgroundColor = Color.green;
    //    if (GUILayout.Button("FIND ALL Glyphs"))
    //    {
    //        for (int i = 0; i < SaveDataManager.glyphs.items.Length; i++)
    //            SaveDataManager.glyphs.gain(i);
    //    }
    //    GUI.backgroundColor = Color.red;
    //    if (GUILayout.Button("LOSE ALL Glyphs"))
    //    {
    //        for (int i = 0; i < SaveDataManager.glyphs.items.Length; i++)
    //            SaveDataManager.glyphs.set(i, false);
    //    }
    //    GUI.backgroundColor = Color.white;
    //}

}
