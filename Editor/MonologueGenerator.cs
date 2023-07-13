using UnityEngine;
using UnityEditor;
using System.IO;
using VesselText;

public class MonologueGenerator : EditorWindow
{
    string[] paths = new string[]
    {
        "C:\\Users\\mwojt\\Documents\\Lopside Games\\Development\\Sardine\\Documentation\\Audio\\eld-subtitle-guide.csv",
        "C:\\Users\\mwojt\\Documents\\Lopside Games\\Development\\Sardine\\Documentation\\Audio\\zol-subtitle-guide.csv",
        "C:\\Users\\mwojt\\Documents\\Lopside Games\\Development\\Sardine\\Documentation\\Audio\\vir-subtitle-guide.csv",
        "C:\\Users\\mwojt\\Documents\\Lopside Games\\Development\\Sardine\\Documentation\\Audio\\unk-subtitle-guide.csv"
    };

    int pathSelection;
    string filePath = "";
    int speaker;
    string monologueSavePath = "Assets/Vessels/Monologues/";
    string contentPath = "Assets/Resources/VesselContent/";
    public TableOfContents Guide;

    [MenuItem("CustomUtilities/Monologue Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MonologueGenerator));
    }

    void OnGUI()
    {
        Guide = TableOfContents.get();


        GUILayout.Label("CSV File Path", EditorStyles.boldLabel);
        pathSelection = EditorGUILayout.Popup("File Path:", pathSelection, paths);
        filePath = paths[pathSelection];
        speaker = EditorGUILayout.IntSlider("Speaker: ", speaker, 0, Guide.speakers.Length-1);
        EditorGUILayout.LabelField(Guide.getSpeaker(speaker));

        if (GUILayout.Button("Create Monologues"))
        {
            CreateMonologues();
        }
    }

    void CreateMonologues()
    {
        if (File.Exists(filePath))
        {
            
            using (StreamReader reader = new StreamReader(filePath))  // Iterate through CSV
            {
                while (!reader.EndOfStream)  
                {
                    string[] line = reader.ReadLine().Split(',');
                    if (line.Length == 5)
                    {
                        // Read data from line
                        string fileName = line[0];
                        string sceneName = line[1];
                        int episode = int.Parse(line[2]);
                        string audioFileName = line[3];
                        string srtFileName = line[4];

                        // Locate or Create new monologue
                        Monologue monologue = (Monologue)AssetDatabase.LoadAssetAtPath(monologueSavePath + fileName + ".asset", typeof(Monologue));
                        bool isAssetNew = monologue == null;
                        if (isAssetNew)
                        {
                            //Directory.CreateDirectory(savePath);
                            monologue = ScriptableObject.CreateInstance<Monologue>();
                        }

                        // Assign property values
                        monologue.name = fileName;
                        monologue.title = sceneName;
                        monologue.episode = episode;
                        monologue.voClip = AssetDatabase.LoadAssetAtPath<AudioClip>(contentPath + fileName.Substring(0, 3) + "/" + audioFileName + ".wav");
                        Debug.Log(contentPath + fileName.Substring(0, 3) + "/" + audioFileName);////
                        monologue.setLines(contentPath + fileName.Substring(0, 3) + "/" + srtFileName);
                        monologue.speaker = speaker;

                        if (isAssetNew)
                            AssetDatabase.CreateAsset(monologue, monologueSavePath + fileName + ".asset");

                        EditorUtility.SetDirty(monologue);

                        Debug.Log("Successfully created Monologue asset at " + monologueSavePath + fileName);
                    }
                    else
                    {
                        Debug.LogWarning("Line in CSV file does not have exactly 4 columns: " + string.Join(",", line));
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogError("File not found at path: " + filePath);
        }
    }
}

/*
 * Written by ChatGPY from below prompts. Definitely some issues, but a great starting point for revision.
 * I just had to move some pieces around, based on the logic of what I was doing. ChatGPT somewhat misunderstood,
 * seeming to "think" that the monologues were part of one monologue asset.
 * 
 * It probably saved me around a half hour to an hour. The big benefit is that it knows all the "incantations" - 
 * what lines of code achieve what behavior, saving me time from looking them up myself individually. For example,
 * I didn't need to check how to load in or save assets and the proper lines to do that.
 * 
 * Also, I technically lied when I said there were only four columns - there are actually five. But, that didn't seem 
 * to confuse ChatGPT too much.
 * 
 * Sure, here's an example of how to create a custom editor window in Unity C# that allows the user to specify a file path to a CSV file and then iterates through the file's contents and outputs each piece of data using Debug.Log:
 * 
 * Update the code to include the following functionality: The CSV is expected to have four columns. The first column represents the "name" field. At each line of the CSV, create a new instance of some scriptable object called "Monologue." Its name should be the name in the CSV's name field. These created Monologue scriptable objects should be saved to a consistent file location. If a file of that name already exists, override it. If it does not already exist, then just create the new file.
 * 
 * Update the code to implement this functionality: The first column of the CSV file is the scriptable object's name, as you have already implemented. The second column is the episode name - that value (a string) should be ascribed to the scriptable object's "episode" property. The third column is the chapter - that value (an integer) should be ascribed to the scriptable object's "chapter" property. The third and fourth columns represent file names. The string in column three is the file name of an audio clip. Append that filename to the directory path, and assign the found clip to the scriptable object's "clip" property (an AudioClip value). Finally, the fourth column is the file name of an SRT file. Simply pass the directory path plus that file name into the scriptable object's method call of "setLines()". Let me know if you have any questions in order to implement this. Accuracy is important.
 
    *** EDIT: I spent about a half hour to an hour debugging an issue which seemed to just be caused by not saving the scriptable objects as .asset.

    Here is the final code from ChatGPT, which I modified:

using UnityEngine;
using UnityEditor;
using System.IO;

public class CustomEditorWindow : EditorWindow {

    string filePath = "";
    string fileName = "Monologues.asset";
    string savePath = "Assets/Scripts/Monologues/";

    [MenuItem("Window/Custom Editor Window")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(CustomEditorWindow));
    }

    void OnGUI() {
        GUILayout.Label("CSV File Path", EditorStyles.boldLabel);
        filePath = EditorGUILayout.TextField("File Path:", filePath);

        if(GUILayout.Button("Create Monologues")) {
            CreateMonologues();
        }
    }

    void CreateMonologues() {
        if(File.Exists(filePath)) {
            ScriptableObject monologuesAsset = AssetDatabase.LoadAssetAtPath(savePath + fileName, typeof(ScriptableObject));
            Monologues monologues;
            if(monologuesAsset == null) {
                Directory.CreateDirectory(savePath);
                monologues = ScriptableObject.CreateInstance<Monologues>();
                AssetDatabase.CreateAsset(monologues, savePath + fileName);
                AssetDatabase.SaveAssets();
            }
            else {
                monologues = (Monologues) monologuesAsset;
            }
            using(StreamReader reader = new StreamReader(filePath)) {
                while(!reader.EndOfStream) {
                    string[] line = reader.ReadLine().Split(',');
                    if(line.Length == 4) {
                        string name = line[0];
                        string episode = line[1];
                        int chapter = int.Parse(line[2]);
                        string audioFileName = line[3];
                        string srtFileName = line[4];

                        Monologue newMonologue = ScriptableObject.CreateInstance<Monologue>();
                        newMonologue.name = name;
                        newMonologue.episode = episode;
                        newMonologue.chapter = chapter;
                        newMonologue.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(savePath + audioFileName);
                        newMonologue.setLines(savePath + srtFileName);

                        monologues.monologues.Add(newMonologue);
                    }
                    else {
                        Debug.LogWarning("Line in CSV file does not have exactly 4 columns: " + string.Join(",", line));
                    }
                }
            }
            EditorUtility.SetDirty(monologues);
            AssetDatabase.SaveAssets();
            Debug.Log("Successfully created Monologues asset at " + savePath + fileName);
        }
        else {
            Debug.LogError("File not found at path: " + filePath);
        }
    }
}

 
 
 */