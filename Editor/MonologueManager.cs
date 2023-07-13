using UnityEngine;
using UnityEditor;
using System;
using VesselText;
using System.Collections.Generic;

public class MonologueManager : EditorWindow
{
    TableOfContents Guide;
    public Transform VesselParent;
    bool[] speakerFilter;
    bool[] chapterFilter;
    bool[] episodeFilter;
    //bool[] sectionFilter;
    bool includeAll = false;
    bool displaySelectedOnly = false;
    bool includeInScene = false;
    bool showMonologueProperties = false;
    Monologue monologueActiveInScene;

    public Vector2 scrollFilter1, scrollFilter2, scrollFilter3;
    public Vector2 monologueScroll;

    public Dictionary<Monologue, GameObject> allMonologuesInScene;

    Monologue infoMonologue;
    Rect popupRect = new Rect(20, 20, 200, 200);

    GUIStyle labelStyle;

    [MenuItem("Custom Utilities/MonologueManager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MonologueManager));
    }

    private void OnGUI()
    {
        getGuide();
        findVesselsInScene();
        checkFields();

        filterOptions();
        EditorGUILayout.Space();

        findMonologueActiveInScene();
        if (displaySelectedOnly)
            displaySelectedMonologue();
        else
        {
            setAllMonologuesInScene();
            displayMonologues();
        }

        //displayCurrentSelection();
    }

    /** getGuide and findVesselsInScene locate certain necessary objects. **/
    void getGuide()
    {
        Guide = TableOfContents.get();

        if (VesselParent == null)
        {
            EditorGUILayout.LabelField(TableOfContents.getErrorMsg());
        }
    }
    void findVesselsInScene()
    {
        VesselParent = GameObject.FindGameObjectWithTag("Vessels")?.transform; // name?------

        if (VesselParent == null)
        {
            EditorGUILayout.LabelField("Couldn't find vessel parents in scene. Vessels will not be spawnable right now.");
        }
    }

    /** filterOptions determines which Vessels user is interested in. **/
    public void filterOptions()
    {
        EditorGUILayout.LabelField("Filters", EditorStyles.boldLabel);

        // Filter Flags
        if (!displaySelectedOnly && !includeInScene)
            includeAll = EditorGUILayout.Toggle("Include All", includeAll);
        if (includeAll)
            return;

        if (!includeAll && !displaySelectedOnly)
            includeInScene = EditorGUILayout.Toggle("Include All in Scene", includeInScene);
        if (includeInScene)
            return;

        displaySelectedOnly = EditorGUILayout.Toggle("Show Selected Only", displaySelectedOnly);
        if (displaySelectedOnly)
            return;

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        scrollFilter1 = EditorGUILayout.BeginScrollView(scrollFilter1, GUILayout.Width(200), GUILayout.Height(200));
        EditorGUILayout.LabelField("Scenes");
        filterOption(Guide.Chapters, ref episodeFilter);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        //EditorGUILayout.BeginVertical();
        //scrollFilter2 = EditorGUILayout.BeginScrollView(scrollFilter2, GUILayout.Width(200), GUILayout.Height(200));
        //EditorGUILayout.LabelField("Chapters");
        //filterOption(Guide.chapterNames, ref chapterFilter);
        //EditorGUILayout.EndScrollView();
        //EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        scrollFilter3 = EditorGUILayout.BeginScrollView(scrollFilter3, GUILayout.Width(200), GUILayout.Height(200));
        EditorGUILayout.LabelField("Speakers");
        filterOption(Guide.speakers, ref speakerFilter);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();




        void filterOption(string[] options, ref bool[] filter)
        {
            //Debug.Log(options.Length + " " + filter.Length);//
            for (int i = 0; i < options.Length; i++)
            {
                filter[i] = EditorGUILayout.Toggle(options[i], filter[i]);
            }
        }
    }


    /** checkFields ensures we have valid start data. **/
    void checkFields()
    {
        // Set label's style
        labelStyle = new GUIStyle();
        labelStyle.normal = new GUIStyleState();
        labelStyle.normal.textColor = Color.black;
        labelStyle.normal.background = Texture2D.whiteTexture;

        // Check filters
        if (!(checkArrays(speakerFilter, Guide.speakers) && checkArrays(chapterFilter, Guide.Chapters) && checkArrays(episodeFilter, Guide.Chapters)))
            setFiltersToNone();

        bool checkArrays<T, U>(T[] filterArray, U[] contentArray)
        {
            if (contentArray == null)
            {
                EditorGUILayout.LabelField("ERROR: An array in the Table of Content scriptable object is not set.");
            }
            //Debug.Log(filterArray.Length + " " + contentArray.Length);
            return filterArray != null && contentArray != null && filterArray.Length == contentArray.Length;
        }

        void setFiltersToNone()
        {
            speakerFilter = new bool[Guide.speakers.Length];
            chapterFilter = new bool[Guide.Sections.Length];
            episodeFilter = new bool[Guide.Chapters.Length];
            //sectionFilter = new bool[Guide.sections.Length];
        }

        void setFiltersToAll()
        {
            speakerFilter = new bool[Guide.speakers.Length];
            setArray(speakerFilter, true);
            chapterFilter = new bool[Guide.Sections.Length];
            setArray(chapterFilter, true);
            episodeFilter = new bool[Guide.Chapters.Length];
            setArray(episodeFilter, true);
            //sectionFilter = new bool[Guide.sections.Length];
            //setArray(sectionFilter, true);
        }

        // Helper method
        void setArray<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }
    }


    public void displayMonologues()
    {
        GUILayout.ExpandHeight(true);
        monologueScroll = EditorGUILayout.BeginScrollView(monologueScroll, GUILayout.Height(600));
        bool inScene;

        for (int i = 0; i < Guide.Monologues.Length; i++)
        {
            inScene = allMonologuesInScene.ContainsKey(Guide.Monologues[i]);
            if (checkFilters(Guide.Monologues[i], inScene))
                displayMonologue(Guide.Monologues[i], inScene, i);
        }



        EditorGUILayout.EndScrollView();
        GUILayout.ExpandHeight(false);


        bool checkFilters(Monologue mono, bool inScene)
        {
            // Returns true if include all OR all the filters are satisfactory.
            if (includeAll)
                return true;
            if (includeInScene && !inScene)
                return false;
            return (speakerFilter[mono.speaker] || allFalse(speakerFilter)) && (chapterFilter[mono.chapter] || allFalse(chapterFilter)) && (episodeFilter[mono.episode] || allFalse(episodeFilter));
            // For now, don't worry about section filter.
        }
    }

    void displayMonologue(Monologue mono, bool sceneContainsMono, int index)
    {
        EditorGUILayout.BeginHorizontal();
        numberField();
        spawnButton(mono);
        setStyle();
        GUILayout.Label(mono.title, labelStyle, GUILayout.Width(125));
        GUILayout.Label(Guide.getChapterName(mono.episode), labelStyle, GUILayout.Width(125));
        GUILayout.Label(Guide.getSpeaker(mono.speaker), labelStyle, GUILayout.Width(50));
        //GUILayout.Label(Guide.getChapterName(mono.chapter));
        infoButton();

        void numberField()
        {
            if (index < 0)
                return;
            
            int newIndex = 0;
            newIndex = EditorGUILayout.DelayedIntField(index, GUILayout.Width(35));

            if (newIndex != index)
            {
                Guide.moveMonologue(index, newIndex);
            }
        }

        void spawnButton(Monologue mono)
        {
            // If currently selected
            if (mono == monologueActiveInScene)
            {
                GUI.backgroundColor = Color.blue;
                if (GUILayout.Button("DESELECT", GUILayout.Width(75)))
                {
                    Selection.activeGameObject = null;
                }
                return;
            }


            string message = "Spawn";
            if (sceneContainsMono)
            {
                GUI.backgroundColor = Color.blue;
                message = "Select";
            }
            else
            {
                GUI.backgroundColor = Color.white;
            }

            if (GUILayout.Button(message, GUILayout.Width(75)))
            {
                if (sceneContainsMono)
                {
                    // select
                    Selection.activeGameObject = allMonologuesInScene[mono];
                }
                else
                {
                    // spawn
                    spawnVessel(mono);
                }
            }
        }

        void infoButton()
        {
            bool isMonoUnselected = infoMonologue == null || infoMonologue != mono;
            string infoButtonDisplay = ". . .";
            if (!isMonoUnselected)
                infoButtonDisplay = "X";

            if (GUILayout.Button(infoButtonDisplay, GUILayout.Width(25)))
            {
                EditorGUILayout.EndHorizontal();
                if (isMonoUnselected)
                {
                    infoMonologue = mono;
                    drawSelectedMonologue(infoMonologue);
                }
                else
                {
                    infoMonologue = null;
                }
            }
            else
            {
                EditorGUILayout.EndHorizontal();
                if (infoMonologue == mono)
                {
                    drawSelectedMonologue(infoMonologue);
                }
            }
        }

        void setStyle()
        {
            if (mono == monologueActiveInScene)
            {
                labelStyle.normal.textColor = Color.white;
                GUI.backgroundColor = Color.blue;
                labelStyle.normal.background = Texture2D.whiteTexture;
            }
            else if (mono == infoMonologue)
            {
                labelStyle.normal.textColor = Color.black;
                GUI.backgroundColor = Color.yellow;
                labelStyle.normal.background = Texture2D.whiteTexture;
            }
            else
            {
                labelStyle = new GUIStyle("label");
                GUI.backgroundColor = Color.white;
            }
        }

    }

    bool allFalse(bool[] array)
    {
        bool result = true;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i])
                result = false;
        }
        return result;
    }

    public GUILayoutOption flexWidth(int shares)
    {
        if (shares <= 0)
            shares = 1;

        return GUILayout.Width(EditorGUIUtility.currentViewWidth / shares);
    }
    public GUILayoutOption flexWidth(float other, int shares)
    {
        if (shares <= 0)
            shares = 1;

        return GUILayout.Width((EditorGUIUtility.currentViewWidth - other) / shares);
    }

    void drawSelectedMonologue(Monologue mono)
    {
        // Display Subtitles
        GUI.backgroundColor = Color.white;
        foreach (Subtitle S in mono.Subtitles)
        {
            EditorGUILayout.LabelField(S.text.Replace("\n", " "));
        }

        // On button press, show the Monologue's inspector
        string buttonLabel = "Show Inspector";
        if (showMonologueProperties)
            buttonLabel = "Hide Inspector";
        if (GUILayout.Button(buttonLabel))
        {
            showMonologueProperties = !showMonologueProperties;
        }

        if (showMonologueProperties)
        {
            GUI.backgroundColor = Color.yellow;
            Editor monologueEditor = Editor.CreateEditor(mono);
            monologueEditor.OnInspectorGUI();
            EditorGUILayout.ObjectField("Scriptable Object", mono, typeof(Monologue));
            GUI.backgroundColor = Color.white;
        }
    }

    public void setAllMonologuesInScene()
    {
        GameObject[] allGameObjectsWithTag = GameObject.FindGameObjectsWithTag("Vessels");
        allMonologuesInScene = new Dictionary<Monologue, GameObject>();
        for (int i = 0; i < allGameObjectsWithTag.Length; i++)
        {
            Monologue foundMono = allGameObjectsWithTag[i].GetComponent<Pickup>()?.monolouge;
            if (foundMono != null)
            {
                // Add monologue and corresponding game object
                allMonologuesInScene[foundMono] = allGameObjectsWithTag[i];
            }
        }
    }

    void spawnVessel(Monologue mono)
    {
        string path = "Prefabs/ShellPickups/Shell";
        path += "-" + Guide.getSpeaker(mono.speaker) + " Variant";
        Debug.Log(path);//

        GameObject prefab = Resources.Load(path) as GameObject;
        GameObject vessel = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        vessel.transform.position = getPosition(2.5f);
        //frag.transform.parent = tabletsOverworldParent;
        vessel.GetComponent<Pickup>().monolouge = mono;
        vessel.name = "[Collect] " + mono.title + " | " + Guide.getSpeaker(mono.speaker) + " | " + Guide.getChapterName(mono.episode);

        Vector3 getPosition(float spawnDistance)
        {
            Vector3 cameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
            Vector3 cameraForward = SceneView.lastActiveSceneView.camera.transform.forward;
            return cameraPosition + (cameraForward * spawnDistance);
        }
    }

    // Make sure to run findMonologueActiveInScene() before this function
    void displaySelectedMonologue()
    {
        
        if (monologueActiveInScene == null)
        {
            EditorGUILayout.LabelField("No GameObject with monologue selected.");
            return;
        }

        displayMonologue(monologueActiveInScene, true, -1);
    }

    void findMonologueActiveInScene()
    {
        GameObject GO = Selection.activeGameObject;

        // If nothing in selection, exit
        if (GO == null)
        {
            monologueActiveInScene = null;
            return;
        }

        Pickup pickup;

        // If pickup on selected game object, exit
        pickup = GO.GetComponent<Pickup>();
        if (pickup != null)
        {
            foundSelection(pickup.monolouge);
            return;
        }

        // Last chance, check child
        GameObject child = GO.transform.GetChild(0)?.gameObject;
        if (child != null)
        {
            pickup = child.GetComponent<Pickup>();
            if (pickup != null)
            {
                foundSelection(pickup.monolouge);
            }
        }

        void foundSelection(Monologue m)
        {
            if (m != monologueActiveInScene)  // The monologue was recently selected
            {
                infoMonologue = m;  // ...so show the subtitle by default
                //showMonologueProperties = true;
            }

            monologueActiveInScene = m;
        }
    }

}

//https://www.google.com/search?q=unity+gui+field+wait+til+done+typing&oq=unity+gui+field+wait+til+done+typing&aqs=edge..69i57j0i546l4.7036j0j9&sourceid=chrome&ie=UTF-8
//https://answers.unity.com/questions/41841/how-can-i-determine-when-user-presses-enter-in-an.html
//https://forum.unity.com/threads/gui-textfield-submission-via-return-key.69361/