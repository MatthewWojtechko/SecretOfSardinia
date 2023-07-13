using UnityEngine;
using UnityEditor;

public class Grounder : EditorWindow
{
    [MenuItem("Custom Utilities/Grounder")]
    public static void ShowWindow()
    {
        GetWindow<Grounder>("Grounder");
    }

    public enum Mode { LOCAL, WORLD, PROXIMITY };
    public enum Axis { UP, DOWN, LEFT, RIGHT, FORWARD, BACKWARD };
    public LayerMask layerMask;
    public Mode mode;
    public Axis axis;
    public float proximityRadius;
    public bool alignWithNormal = false;


    private void OnGUI()
    {
        LayerMask tempMask = EditorGUILayout.MaskField("Layer Mask", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMask), UnityEditorInternal.InternalEditorUtility.layers);
        layerMask = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

        mode = (Mode)EditorGUILayout.EnumPopup("Mode", mode);
        if (mode == Mode.PROXIMITY)
        {
            proximityRadius = EditorGUILayout.FloatField("Radius", proximityRadius);
        }
        else
        {
            axis = (Axis)EditorGUILayout.EnumPopup("Axis", axis);
        }

        alignWithNormal = EditorGUILayout.Toggle("Align With Normal", alignWithNormal);

        if (GUILayout.Button("Ground"))
        {
            GroundObjects();
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (mode == Mode.PROXIMITY)
            return;

        foreach (GameObject obj in Selection.gameObjects)
        {
            Vector3 direction = getDirection_worldSpace(obj);

            if (mode == Mode.WORLD)
                Debug.DrawLine(obj.transform.position, obj.transform.position + (direction * 10));
            else if (mode == Mode.LOCAL)
                Debug.DrawLine(obj.transform.position, obj.transform.position + (direction * -10));
        }
    }

    Vector3 getDirection_worldSpace(GameObject go)
    {
        if (mode == Mode.WORLD)
        {
            switch (axis)
            {
                case Axis.UP:
                    return Vector3.up;

                case Axis.DOWN:
                    return Vector3.down;

                case Axis.RIGHT:
                    return Vector3.right;

                case Axis.LEFT:
                    return Vector3.left;

                case Axis.FORWARD:
                    return Vector3.forward;

                case Axis.BACKWARD:
                    return Vector3.back;
            }
        }
        else if (mode == Mode.LOCAL)
        {
            switch (axis)
            {
                case Axis.UP:
                    return go.transform.up;

                case Axis.DOWN:
                    return go.transform.up * -1;

                case Axis.RIGHT:
                    return go.transform.right;

                case Axis.LEFT:
                    return go.transform.right * -1;

                case Axis.FORWARD:
                    return go.transform.forward;

                case Axis.BACKWARD:
                    return go.transform.forward * -1;
            }
        }
        return Vector3.zero;
    }

    void alignGameObject(GameObject obj, Vector3 normal)
    {
        switch (axis)
        {
            case Axis.UP:
                obj.transform.up = normal;
                break;

            case Axis.DOWN:
                obj.transform.up = -normal;
                break;

            case Axis.RIGHT:
                obj.transform.right = normal;
                break;

            case Axis.LEFT:
                obj.transform.right = -normal;
                break;

            case Axis.FORWARD:
                obj.transform.forward = normal;
                break;

            case Axis.BACKWARD:
                obj.transform.forward = -normal;
                break;
        }
    }

    private void GroundObjects()
    {
        Undo.RecordObjects(Selection.gameObjects, "Ground Objects");

        foreach (GameObject obj in Selection.gameObjects)
        {
            switch (mode)
            {
                case Mode.LOCAL:
                case Mode.WORLD:

                    Vector3 direction = getDirection_worldSpace(obj);

                    RaycastHit hit;
                    if (Physics.Raycast(obj.transform.position, direction, out hit, Mathf.Infinity, layerMask))
                    {
                        obj.transform.position = hit.point;
                        if (alignWithNormal)
                        {
                            alignGameObject(obj, hit.normal);
                        }
                    }
                    break;
                case Mode.PROXIMITY:
                    Collider[] colliders = Physics.OverlapSphere(obj.transform.position, proximityRadius, layerMask);
                    if (colliders.Length > 0)
                    {
                        float minDistance = float.MaxValue;
                        Vector3 closestPoint = Vector3.zero;
                        foreach (Collider collider in colliders)
                        {
                            Vector3 point = collider.ClosestPoint(obj.transform.position);
                            float distance = Vector3.Distance(obj.transform.position, point);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                closestPoint = point;
                            }
                        }
                        obj.transform.position = closestPoint;
                        if (alignWithNormal)
                        {
                            obj.transform.up = (closestPoint - obj.transform.position).normalized;
                        }
                    }
                    break;
            }
        }
    }




}
// Assisted by: https://chat.openai.com/chat/ca2ad25d-b8db-4f51-a503-a6c1464316cc