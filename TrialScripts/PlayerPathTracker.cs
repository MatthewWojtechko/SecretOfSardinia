using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPathTracker : MonoBehaviour
{
    public Vector3[] points = new Vector3[30];
    public int numPoints = 0;
    public float minimumAngleDifference = 5;
    public float timeStep = 0.2f;
    public Vector3 secondRecentPoint;
    public Vector3 mostRecentPoint;
    public Vector3 currentPoint;
    public GameObject waypointPrefab;

    public bool isRecording = false;
    public float lastCheckTime = 0;

    void addNewPoint(Vector3 newPoint)
    {
        checkArraySize();
        ////Debug.Log(newPoint);//
        points[numPoints++] = newPoint;

        secondRecentPoint = mostRecentPoint;
        mostRecentPoint = newPoint;
    }

    //void updateLast(Vector3 newPoint)
    //{
    //    points[numPoints] = newPoint;
    //    mostRecentPoint = newPoint;
    //}

    void checkPoints()
    {
        currentPoint = SardineSwim.playerTransform.position;
        Vector3 currentDirection = (secondRecentPoint - currentPoint).normalized;
        Vector3 oldDirection = (secondRecentPoint - mostRecentPoint).normalized;
        float angle = Vector3.Angle(currentDirection, oldDirection);
        if (Mathf.Abs(angle) >= minimumAngleDifference || ((secondRecentPoint == mostRecentPoint) && currentPoint != mostRecentPoint))  // The current angle is different enough
        {
            //Debug.Log(angle);
            addNewPoint(currentPoint);
        }
        //else
        //{
        //    updateLast(currentPoint);
        //}
    }



    public void begin()
    {
        secondRecentPoint = mostRecentPoint = SardineSwim.playerTransform.position;
        //addNewPoint(SardineSwim.playerTransform.position);
        //addNewPoint(SardineSwim.playerTransform.position);
        isRecording = true;
    }

    public void end()
    {
        addNewPoint(SardineSwim.playerTransform.position);
        isRecording = false;
    }



    public void instantiateGameobjectsAtPoints()
    {
        for (int i = 0; i < numPoints; i++)
        {
            GameObject waypoint;
            if (i < this.transform.childCount)
            {
                waypoint = this.transform.GetChild(i).gameObject;
            }
            else
            {
                waypoint = GameObject.Instantiate(waypointPrefab);
                waypoint.transform.parent = this.gameObject.transform;
            }
            waypoint.transform.position = points[i];
        }
    }

    void checkArraySize()
    {
        if (numPoints == points.Length)
        {
            Vector3[] newArray = new Vector3[points.Length + 30];
            for (int i = 0; i < points.Length; i++)
            {
                newArray[i] = points[i];
            }
            points = newArray;
        }
    }

    private void Update()
    {
        if (!isRecording)
            return;

        float currentTime = Time.time;
        if (currentTime - lastCheckTime >= timeStep)
        {
            lastCheckTime = currentTime;
            checkPoints();
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null)
                continue;

            if (i == numPoints - 1)
                Gizmos.color = Color.red;
            else if (i == numPoints - 2)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(points[i], 0.2f);
        }
    }


}