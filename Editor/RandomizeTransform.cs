using UnityEngine;
using UnityEditor;

public class RandomizeTransform : EditorWindow
{
    [MenuItem("CustomUtilities/RandomizeTransform")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RandomizeTransform));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Randomize Local Y Rotation"))
        {
            foreach (GameObject G in Selection.gameObjects)
            {
                G.transform.localRotation = Quaternion.Euler(G.transform.localRotation.x, Random.Range(0, 360), G.transform.localRotation.z);
            }
        }
    }
}
