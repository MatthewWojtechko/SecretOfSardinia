using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class FragmentInspector : EditorWindow
{
    public Transform tabletsCollectionParent;
    public Transform tabletsOverworldParent;

    public int[] tabletsToCheck = new int[14];
    string[] tabletNames;
    int numTabletsToCheck = 1;
    bool checkAllTablets = false;

    string inputString;
    string[] searchWords;
    int[] targetTerms;
    int[] overallWordCounts = { };

    string wordSearchErrorMsg;
    bool hasWordSearchError = false;

    bool checkingAllGlyphs = false;

    Vector2 resultsScroll = Vector2.zero;
    Vector2 summaryScroll = Vector2.zero;

    List<SearchResult> fragmentSearchResults = new List<SearchResult>();

    bool filterByTablet = true;

    string[] tabs = new string[] { "Totals", "Fragments" };
    int currentTab = 0;

    public class SearchResult
    {
        public int[] wordCounts;
        public int tablet, fragment;
        public bool isHidden = false;
    }
  

    [MenuItem("Custom Utilities/Fragment Inspector")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FragmentInspector));
    }

    private void OnGUI()
    {
        getParents();
        if (tabletsCollectionParent == null || tabletsOverworldParent == null)
        {
            EditorGUILayout.LabelField("Error: Couldn't find tablet parents in scene. Make sure the translatable tablets are parented to a game object tagged as \"TabletsTranslation\" and the hidden tablets are parented to a game object tagged as \"TabletsOverworld\"");
            return;
        }

        filterOption();

        if (filterByTablet)
            tabletsOption();

        EditorGUILayout.Space(10);
        wordInput();

        wordSearchButton();
        if (hasWordSearchError)
            EditorGUILayout.LabelField(wordSearchErrorMsg);

        EditorGUILayout.Space(30);
        tabOption();

        if (currentTab == 0)
            displaySummary(); 
        else
            displayFragmentResults();
    }

    void tabOption()
    {
        currentTab = GUILayout.Toolbar(currentTab, tabs);
    }

    void getParents()
    {
        tabletsCollectionParent = GameObject.FindGameObjectWithTag("TabletsTranslation")?.transform;
        tabletsOverworldParent = GameObject.FindGameObjectWithTag("TabletsOverworld")?.transform;
    }

    public void filterOption()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 60;
        EditorGUILayout.LabelField("Filter By: ", EditorStyles.boldLabel);
        filterByTablet = EditorGUILayout.Toggle("Tablet", filterByTablet);
        filterByTablet = !EditorGUILayout.Toggle("Selection", !filterByTablet);
        EditorGUILayout.EndHorizontal();
    }



    void tabletsCollectionParentField()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tablets Translation Parent");
        tabletsCollectionParent = EditorGUILayout.ObjectField(tabletsCollectionParent, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();
    }

    void tabletsOverworldParentField()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tablets Overworld Parent");
        tabletsOverworldParent = EditorGUILayout.ObjectField(tabletsOverworldParent, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();
    }

    void tabletsOption()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Number of Tablets");
        if (!checkAllTablets)
            numTabletsToCheck = EditorGUILayout.IntField(numTabletsToCheck);
        checkAllTablets = EditorGUILayout.Toggle("All", checkAllTablets);
        EditorGUILayout.EndHorizontal();

        // Check all tablets
        if (checkAllTablets)
        {
            numTabletsToCheck = tabletsCollectionParent.childCount;
            for (int i = 0; i < tabletsCollectionParent.childCount; i++)
            {
                tabletsToCheck[i] = i;
            }
            return;
        }

        // Let the player select tablets to check
        tabletNames = new string[tabletsCollectionParent.childCount];
        for (int i = 0; i < tabletsCollectionParent.childCount; i++)
        {
            tabletNames[i] = tabletsCollectionParent.GetChild(i).name;
        }

        for (int i = 0; i < numTabletsToCheck && i < tabletsCollectionParent.childCount; i++)
        {
            tabletsToCheck[i] = EditorGUILayout.Popup(tabletsToCheck[i], tabletNames);
        }
    }

    void wordInput()
    {
        EditorGUILayout.LabelField("Words");

        inputString = EditorGUILayout.TextField(inputString);
    }

    void wordSearchButton()
    {
        if (GUILayout.Button("Search"))
        {
            hasWordSearchError = false;
            if (inputString == null || inputString == "")
            {
                //// Error! Whoops!
                //hasWordSearchError = true;
                //wordSearchErrorMsg = "Error: Text field is blank. Type in the words you want to check.";//
                //return;

                // If no input, then do ALL the words
                checkingAllGlyphs = true;
                targetTerms = new int[Cipher.translations.Length];
                for (int i = 0; i < targetTerms.Length; i++)
                {
                    targetTerms[i] = i;
                }
            }
            else
            {
                checkingAllGlyphs = false;
                searchWords = inputString.Split(' ');
                targetTerms = new int[searchWords.Length];
                for (int i = 0; i < targetTerms.Length; i++)
                {
                    targetTerms[i] = Cipher.getWordIndex(searchWords[i]);
                    if (targetTerms[i] == -1)
                    {
                        // Error! Term not found.
                        hasWordSearchError = true;
                        wordSearchErrorMsg = "Error: The word \"" + searchWords[i] + "\" is not included in the language.";
                        return;
                    }
                }
            }

            getSearchResults();
        }
    }

    private void getSearchResults()
    {
        fragmentSearchResults.Clear();

        overallWordCounts = new int[targetTerms.Length];

        resultsScroll = EditorGUILayout.BeginScrollView(resultsScroll);

        if (filterByTablet)  // Cycle through every listed tablet and all its fragments
        {
            for (int tabletIndex = 0; tabletIndex < numTabletsToCheck; tabletIndex++)
            {
                Transform tablet = tabletsCollectionParent.GetChild(tabletsToCheck[tabletIndex]);
                for (int fragIndex = 0; fragIndex < tablet.childCount; fragIndex++) 
                {
                    searchFragment(tabletIndex, fragIndex);
                }
            }
        }
        else  // Cycle through every fragment in the Selection
        {
            GameObject[] selectedGOs = Selection.gameObjects;

            for (int i = 0; i < selectedGOs.Length; i++)
            {
                FragSaveLoad fragData = selectedGOs[i].GetComponent<FragSaveLoad>();
                if (fragData != null)
                    searchFragment(fragData.tabletID, fragData.fragID);
                Debug.Log(i);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void searchFragment(int tabletIndex, int fragIndex)
    {
        Transform fragment = tabletsCollectionParent.GetChild(tabletsToCheck[tabletIndex]).GetChild(fragIndex);

        // Create new search result entry
        SearchResult result = new SearchResult();
        result.wordCounts = new int[targetTerms.Length];
        result.tablet = tabletsToCheck[tabletIndex];
        result.fragment = fragIndex;

        bool hasMatchingGlyphs = false;

        for (int glyphIndex = 0; glyphIndex < fragment.childCount; glyphIndex++)
        {
            Transform glyph = fragment.GetChild(glyphIndex);

            for (int termIndex = 0; termIndex < targetTerms.Length; termIndex++)
            {
                if (Cipher.checkWord(glyph.name, targetTerms[termIndex]))
                {
                    // Match!
                    hasMatchingGlyphs = true;
                    result.wordCounts[termIndex]++;
                    overallWordCounts[termIndex]++;
                    Debug.Log(glyph.name + " " + targetTerms[termIndex]);//

                }
            }
        }

        // Add fragment's matching data to the list
        if (hasMatchingGlyphs)
        {
            fragmentSearchResults.Add(result);
        }
    }

    void spawnFragment(int t, int f)
    {
        GameObject prefab = Resources.Load("Prefabs/FragmentPickup") as GameObject;
        GameObject frag = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        frag.transform.position = getPosition(2.5f);
        frag.transform.parent = tabletsOverworldParent;
        FragSaveLoad fragData = frag.GetComponent<FragSaveLoad>();
        fragData.tabletID = t;
        fragData.fragID = f;
        frag.name = formatFragmentName(t,f);

        Vector3 getPosition(float spawnDistance)
        {
            Vector3 cameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
            Vector3 cameraForward = SceneView.lastActiveSceneView.camera.transform.forward;
            return cameraPosition + (cameraForward * spawnDistance);
        }
    }

    string formatFragmentName(int tablet, int fragment)
    {
        return "Fragment " + tablet + "." + fragment;
    }

    Transform getChild(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == name)
                return parent.GetChild(i);
        }
        return null;
    }

    void displayFragmentResults()
    {
        resultsScroll = EditorGUILayout.BeginScrollView(resultsScroll);
        foreach (SearchResult result in fragmentSearchResults)
        {
            EditorGUILayout.LabelField(formatFragmentName(result.tablet, result.fragment), EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            if (!result.isHidden)
            {
                EditorGUIUtility.labelWidth = 20;
                EditorGUILayout.BeginVertical();

                // Display word data
                for (int i = 0; i < result.wordCounts.Length; i++)
                {
                    // If none, skip
                    if (result.wordCounts[i] == 0)
                        continue;

                    EditorGUILayout.BeginHorizontal();

                    if (checkingAllGlyphs)
                        EditorGUILayout.LabelField(Cipher.translations[i][0]);
                    else
                        EditorGUILayout.LabelField(searchWords[i]);

                    string wordData = "";

                    wordData += getNumSpaced(result.wordCounts[i]);

                    wordData += getNumSpaced(Cipher.glyphGoals[targetTerms[i]]);

                    if (result.wordCounts[i] >= Cipher.glyphGoals[targetTerms[i]])
                    {
                        wordData += "[X]";
                    }

                    EditorGUILayout.LabelField(wordData);

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            result.isHidden = EditorGUILayout.Toggle("Hide", result.isHidden);

            if (GUILayout.Button("Read"))
            {
                readFragment(result);
            }

            // Button - spawn or select
            Transform fragInScene = getChild(tabletsOverworldParent, formatFragmentName(result.tablet, result.fragment));
            if (fragInScene != null)
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = fragInScene.gameObject;
                }
                GUI.backgroundColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Spawn"))
                {
                    spawnFragment(result.tablet, result.fragment);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }
        EditorGUILayout.EndScrollView();
    }

    public void displaySummary()
    {
        if (targetTerms?.Length == 0)
            return;

        //EditorGUILayout.LabelField("Totals", EditorStyles.boldLabel);

        GUI.backgroundColor = Color.white;

        summaryScroll = EditorGUILayout.BeginScrollView(summaryScroll);
        for (int i = 0; i < overallWordCounts.Length; i++)
        {
            // Recalculate word counts
            overallWordCounts[i] = 0;
            foreach (SearchResult R in fragmentSearchResults)
            {
                if (R.isHidden)
                    continue;
                overallWordCounts[i] += R.wordCounts[i];
            }


            // If none, skip
            if (overallWordCounts[i] == 0)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 20;
            if (checkingAllGlyphs)
                EditorGUILayout.LabelField(Cipher.translations[i][0]);
            else
                EditorGUILayout.LabelField(searchWords[i]);

            string wordData = "";
            wordData += getNumSpaced(overallWordCounts[i]);
            wordData += getNumSpaced(Cipher.glyphGoals[targetTerms[i]]);
            if (overallWordCounts[i] >= Cipher.glyphGoals[targetTerms[i]])
            {
                wordData += "[X]";
            }
            EditorGUILayout.LabelField(wordData);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    string getNumSpaced(int num)
    {
        string s = "";
        s += num;
        if (num < 100)
            s += "  ";
        else if (num < 10)
            s += " ";
        s += "      ";

        return s;
    }

    public void readFragment(SearchResult result)
    {
        string text = "";
        Transform tablet = tabletsCollectionParent.GetChild(result.tablet);
        Transform fragment = tablet.GetChild(result.fragment);

        for (int glyphIndex = 0; glyphIndex < fragment.childCount; glyphIndex++)
        {
            text += fragment.GetChild(glyphIndex).name + " ";
        }

        EditorUtility.DisplayDialog(formatFragmentName(result.tablet, result.fragment), text, "Okay");
    }
}
