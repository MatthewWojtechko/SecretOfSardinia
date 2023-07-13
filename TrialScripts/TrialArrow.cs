using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialArrow : MonoBehaviour
{
    static TrialArrow Singleton;
    public LineRenderer line;
    public GameObject graphic;
    public Vector3[] linePositions = new Vector3[2];

    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;
    }

    public static void point(Vector3 direction, Vector3 point)
    {
        Singleton.graphic.SetActive(true);
        Singleton.line.enabled = true;
        Singleton.transform.forward = direction.normalized;

        Singleton.linePositions[0] = Singleton.transform.position;
        Singleton.linePositions[1] = point;
        Singleton.line.SetPositions(Singleton.linePositions);
    }

    public static void stopPoint()
    {
        Singleton.graphic.SetActive(false);
        Singleton.line.enabled = false;
    }
}
