using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArrangeInCircle : EditorWindow
{
    GameObject center;
    GameObject original;
    float radius;
    int numCopies;
    int axis;

    Vector3[] points;
    float lineThickness = 0.5f;
    LineRenderer lineRenderer;
    GameObject[] copies;

    [MenuItem("Custom Utilities/Circle Arranger")]

    public static void ShowWindow()
    {
        GetWindow(typeof(ArrangeInCircle));
    }

    public void OnGUI()
    {
        // Center point
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Center");
        center = EditorGUILayout.ObjectField(center, typeof(GameObject), true) as GameObject;
        if (center != null)
            lineRenderer = center.GetComponent<LineRenderer>();
        EditorGUILayout.EndHorizontal();

        // Original game object to copy
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Original");
        original = EditorGUILayout.ObjectField(original, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

        // Radius
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Radius");
        radius = EditorGUILayout.Slider(radius, 0, 150);
        EditorGUILayout.EndHorizontal();

        // Num copies
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Copies");
        numCopies = EditorGUILayout.IntField(numCopies);
        EditorGUILayout.EndHorizontal();

        // Horizontal
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Is Horizontal?");
        axis = EditorGUILayout.IntSlider(axis, 0, 2);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Arrange"))
            arrange();




        OnSceneGUI();
    }

    private void OnInspectorUpdate()
    {
        setPoints();
    }

    void setPoints()
    {
        if (numCopies <= 0)
            return;
        if (center == null)
            return;

        Vector3 origin = center.transform.position;

        points = new Vector3[numCopies];
        for (int i = 0; i < numCopies; i++)
        {
            // Position spot in ring
            float angle = i * Mathf.PI * 2f / numCopies;
            if (axis == 0)
                points[i] = new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius) + origin;
            else if (axis == 1)
                points[i] = new Vector3(0, Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius) + origin;
            else
                points[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) + origin;
        }
        Debug.Log("points " + points.Length);//
    }

    // Draw on scene:
    // https://answers.unity.com/questions/58018/drawing-to-the-scene-from-an-editorwindow.html
    // Window has been selected
    //void OnFocus()
    //{
    //    // Remove delegate listener if it has previously
    //    // been assigned.
    //    SceneView.duringSceneGui -= this.OnSceneGUI;
    //    // Add (or re-add) the delegate.
    //    SceneView.duringSceneGui += this.OnSceneGUI;
    //}

    //void OnDestroy()
    //{
    //    // When the window is destroyed, remove the delegate
    //    // so that it will no longer do any drawing.
    //    SceneView.duringSceneGui -= this.OnSceneGUI;
    //}

    void OnSceneGUI()
    {
        if (points == null || points.Length < 3)
            return;
        if (lineRenderer == null)
            return;

        Debug.Log("DRAW" + points.Length);//

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        lineRenderer.loop = true;


        //// Do your drawing here using Handles.
        //Handles.BeginGUI();
        //Handles.DrawLine(Vector3.zero, Vector3.one, 10);
        //for (int i = 0; i < numCopies-1; i++)
        //{
        //    Handles.DrawLine(points[i], points[i + 1], lineThickness);
        //}
        //Handles.DrawLine(points[numCopies - 1], points[0]);
        //Handles.EndGUI();
    }

    void arrange()
    {
        if (copies != null)
        {
            for (int i = 0; i < copies.Length; i++)
            {
                GameObject.DestroyImmediate(copies[i]);
            }
        }
        if (points == null || points.Length < 1)
            return;

        copies = new GameObject[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            GameObject copy = GameObject.Instantiate(original);
            copy.transform.position = points[i];
            copies[i] = copy;
        }
    }
}

