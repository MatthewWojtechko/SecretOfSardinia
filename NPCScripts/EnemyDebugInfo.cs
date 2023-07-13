using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class EnemyDebugInfo : MonoBehaviour
{

    public EnemyController Controller;

    static string[] headings;

    public void Start()
    {
        headings = new string[]
        {
            "Name: ",
            "\nPosition: ",
            "\nSpeed: ",
            "\nRaw Target Speed: ",
            "\nForce: ",
            "\nBehavior: ",
            "\nSuspicion: ",
            "\nVibes: ",
            "\nMass: ",
            "\nSight Distance: ",
            "\nPlayer Distance: "
        };

        if (Application.isPlaying)
            EnemyDebugAggregate.add(this);
    }

    public string getString()
    {
        if (headings == null)
            return "";

        return headings[0] + this.gameObject.name +
               headings[1] + this.transform.position +
               headings[2] + Math.Round((double)Controller.rigidbody.velocity.magnitude, 2) +
               headings[3] + Controller.Movement?.getSpeed().getRawSpeed() +
               headings[4] + Controller.Movement?.getSpeed().moveForce +
               headings[5] + Controller.Behavior?.getCurrentState() +
               headings[6] + Controller.Suspicion?.currentState +
               headings[7] + Controller.Vibes.status + 
               headings[8] + Controller.rigidbody.mass +
               headings[9] + Controller.Look.getCurrentSightDistance() +
               headings[10] + Mathf.Sqrt((float)Controller.Look?.getPlayerDistanceSqr());
    }

    //void OnDrawGizmos()
    //{
    //    //if (Selection.Contains(this.gameObject))
    //    //Handles.Label(transform.position, getString());
    //    Handles.Label(transform.position,"Test");

    //}
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyDebugInfo))]
public class EnemyDebugInfoEditor : Editor
{
    public int refreshRate = 10;
    int refreshCounter = 0;

    private void OnSceneGUI()
    {
        refreshCounter++;
        if (refreshCounter != 0)
        {
            refreshCounter++;
            return;
        }
        refreshCounter++;

        EnemyDebugInfo script = (EnemyDebugInfo)target;
        //if (Selection.Contains(script.gameObject))
            Handles.Label(script.transform.position + Vector3.up * 0.5f, script.getString());
    }
}
#endif
