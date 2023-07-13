using UnityEngine;
using UnityEditor;
using System;
using VesselText;

public class LocationMarker : EditorWindow
{
    public UnityEngine.Object parentObject;
    string[] colors = new string[] { "Red", "Blue", "Green", "Yellow" };
    int selectedColor = 0;
    int scale = 10;
    float spawnDistance = 10;

    string enemyParentTag = "Enemies";
    string trialParentTag = "Trials";
    string swellParentTag = "Swells";
    string spireParentTag = "Spires";
    string oracleParentTag = "Oracles";
    string vesselsParentTag = "Vessels";
    string fragParentTag = "TabletsOverworld";

    string enemyPath = "Prefabs/EnemySet";
    string trialPath = "Prefabs/TrialSet";
    string swellPath = "Prefabs/SoulSwell";
    string spirePath = "Prefabs/SpiritSpire";
    string oraclePath = "Prefabs/OracleGate";

    string vesselPath = "Prefabs/PickupRange";
    string vesselScriptableObjPathStart = "Assets/Vessels/MonolougeData/";

    int vesselChapter;
    int vesselCharacter;
    string vesselID = "";
    string vesselText;

    bool emphasizeEnemies, emphasizeTrials, emphasizeSwells, emphasizeSpires, emphasizeOracles, emphasizeVessels, emphasizeFragments;


    [MenuItem("Custom Utilities/Location Marker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LocationMarker));
    }

    GameObject prefab;
    string markerName;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Spawn Distance");
            spawnDistance = EditorGUILayout.FloatField(spawnDistance);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Marker", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Parent GameObject");
            parentObject = EditorGUILayout.ObjectField(parentObject, typeof(UnityEngine.Object), true);
        EditorGUILayout.EndHorizontal();

        selectedColor = EditorGUILayout.Popup("Choose Color", selectedColor, colors);
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Marker Scale");
            scale = EditorGUILayout.IntSlider(scale, 1, 30);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Marker Name");
            markerName = GUILayout.TextField(markerName);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Mark"))
        {
            prefab = Resources.Load("Markers/" + colors[selectedColor] + "_Marker") as GameObject;
            // https://gamedev.stackexchange.com/questions/127963/unity-editor-script-to-instantiate-a-prefab
            // https://answers.unity.com/questions/34610/get-the-position-of-the-editor-camera.html
            GameObject go = Instantiate(prefab, getPosition(), Quaternion.identity);
            go.name = markerName;
            if (parentObject != null && parentObject is GameObject)
            {
                go.transform.parent = (parentObject as GameObject).transform;
            }
            go.transform.localScale = Vector3.one * scale;
        }

        EditorGUILayout.Space(30);
        prefabGenerator();

        EditorGUILayout.Space(30);
        vesselGenerator();

        EditorGUILayout.Space(30);
        objectEmphasizer();
    }

    Vector3 getPosition()
    {
        Vector3 cameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
        Vector3 cameraForward = SceneView.lastActiveSceneView.camera.transform.forward;
        return cameraPosition + (cameraForward * spawnDistance);
    }

    // PREFAB GENERATOR //
    void prefabGenerator()
    {
        EditorGUILayout.LabelField("Spawn Prefab", EditorStyles.boldLabel);

        prefabButton("EnemySet", enemyPath, enemyParentTag);
        prefabButton("TrialSet", trialPath, trialParentTag);
        prefabButton("Swell", swellPath, swellParentTag);
        prefabButton("Spire", spirePath, spireParentTag);
        prefabButton("Oracle", oraclePath, oracleParentTag);
    }

    void prefabButton(string name, string path, string parentTag)
    {
        if (GUILayout.Button(name))
        {
            instantiatePrefab(path, name, parentTag);
        }
    }

    GameObject instantiatePrefab(string location, string name, string parentTag)
    {
        GameObject prefab = Resources.Load(location) as GameObject;
        Debug.Log(prefab);//
        GameObject instantation = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Debug.Log(instantation);//



        instantation.name = name;
        instantation.transform.parent = GameObject.FindGameObjectWithTag(parentTag).transform;
        //instantation.transform.localScale = Vector3.one * scale;
        instantation.transform.position = getPosition();

        if (location == oraclePath)
            instantation.transform.LookAt(SceneView.lastActiveSceneView.camera.transform.position);

        // Select spawned object
        Selection.activeObject = instantation;

        return instantation;
    }

    // VESSEL GENERATOR //
    void vesselGenerator()
    {
        //// GUI Fields
        //vesselRealm = EditorGUILayout.Popup(0, Guide.chapterNames);

        //vesselCharacter = (int)EditorGUILayout.EnumPopup("Speaker", vesselCharacter);
        
        //EditorGUILayout.BeginHorizontal();
        //    EditorGUILayout.PrefixLabel("ID");
        //    vesselID = GUILayout.TextField(vesselID);
        //EditorGUILayout.EndHorizontal();

        //EditorGUILayout.PrefixLabel("Text");
        //vesselText = GUILayout.TextArea(vesselText);

        //if (GUILayout.Button("Spawn Vessel"))
        //{
        //    // Create new MonolougeData to specifications above
        //    Monolouge monoData = (Monolouge)ScriptableObject.CreateInstance("MonolougeData");
        //    monoData.Realm = vesselRealm;
        //    monoData.Speaker = vesselCharacter;
        //    monoData.ID = vesselID;
        //    monoData.subtitleText = new string[] { vesselText };
        //    monoData.subtitleTime = new Vector2[] { new Vector2(0, 30) };

        //    // Place the monoData object into the Project folder
        //    AssetDatabase.CreateAsset(monoData, vesselScriptableObjPathStart + monoData.Realm.ToString() + "/" + monoData.ID + ".asset");

        //    // Instantiate a new vessel (make sure the vessel prefab is under Resources/Prefabs, and put that path in a global variable)
        //    //Debug.Log(vesselPath);//
        //    GameObject pickupRange = instantiatePrefab("Prefabs/PickupRange", "[Vessel Range] " + vesselID, vesselsParentTag);

        //    // Assign the scriptable object to the vessel instantiation
        //    pickupRange.transform.GetChild(0).gameObject.GetComponent<Pickup>().monolouge = monoData;
        //}
    }

    public void objectEmphasizer()
    {
        //EditorGUILayout.BeginHorizontal();
            emphasizeFragments = toggle(emphasizeFragments, "Fragments");
            emphasizeEnemies = toggle(emphasizeEnemies, "Enemies");
            emphasizeOracles = toggle(emphasizeOracles, "Oracles");
            emphasizeSpires = toggle(emphasizeSpires, "Spires");
            emphasizeSwells = toggle(emphasizeSwells, "Swells");
            emphasizeTrials = toggle(emphasizeTrials, "Trials");
            emphasizeVessels = toggle(emphasizeVessels, "Vessels");
        //EditorGUILayout.EndHorizontal();
    }

    public bool toggle(bool value, string label)
    {
        bool returnValue;
        //EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label);
            returnValue = EditorGUILayout.Toggle(value);
        //EditorGUILayout.EndVertical();
        return returnValue;
    }

    // Window has been selected
    void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    public void OnSceneGUI(SceneView view)
    {
        if (emphasizeFragments)
            drawEmphasis(fragParentTag, Color.green);
        if (emphasizeEnemies)
            drawEmphasis(enemyParentTag, Color.red);
        if (emphasizeVessels)
            drawEmphasis(vesselsParentTag, Color.blue);
        if (emphasizeSwells)
            drawEmphasis(swellParentTag, Color.cyan);
        if (emphasizeOracles)
            drawEmphasis(oracleParentTag, Color.white);
        if (emphasizeSpires)
            drawEmphasis(spireParentTag, Color.yellow);
    }

    void drawEmphasis(String parentTag, Color color)
    {
        Transform parent = GameObject.FindGameObjectWithTag(parentTag).transform;

        Vector3 size = new Vector3(1, 1, 1);
        Handles.color = color;
        for (int i = 0; i < parent.childCount; i++)
        {
            Handles.DrawWireCube(parent.GetChild(i).position, size);
        }
    }

    //void labelField(string label, Action field)
    //{
    //    EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.PrefixLabel("Marker Name");
    //        field();
    //    EditorGUILayout.EndHorizontal();
    //}
}