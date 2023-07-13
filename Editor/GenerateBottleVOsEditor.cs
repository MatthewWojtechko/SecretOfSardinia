
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GenerateBottleVOs))]
public class GenerateBottleVOsEditor : Editor
{
    DialogData dialogData;
    //SerializedProperty subtitles;
    //private void OnEnable()
    //{
    //    // This links the _phase SerializedProperty to the according actual field
    //    subtitles = serializedObject.FindProperty("subtitles");
    //}
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        GenerateBottleVOs script = (GenerateBottleVOs)target;

        void assign()
        {
            var dialogDataSerialized = new SerializedObject(dialogData);
            dialogDataSerialized.FindProperty("subtitleText").arraySize = script.getLength();
            dialogDataSerialized.FindProperty("subtitleTime").arraySize = script.getLength();
            dialogDataSerialized.ApplyModifiedProperties();
            dialogData.subtitleText = script.getTexts();
            dialogData.subtitleTime = script.getTimes();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (GUILayout.Button("Generate"))
        {
            dialogData = script.target.GetComponent<DialogData>();
            script.generate();
            assign();
        } // https://answers.unity.com/questions/1620506/editor-script-does-not-save-changes.html
        else if (GUILayout.Button("Generate Range"))
        {
            for (int i = script.rangeStart; i < script.rangeEnd; i++)
            {
                dialogData = script.targets[i].GetComponent<DialogData>();

                script.generate(i);
                assign();
            }
        }

    }
}
