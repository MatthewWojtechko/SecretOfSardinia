using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DistanceFromPath : MonoBehaviour
{
    public float innerRadius = 3f;
    public float outerRadius = 9f;
    public Vector3 playerCorrection;
    int currentWaypoint = 0;

    public Vector3 pathPoint;
    public bool isInCloseRange = false;
    public bool isInFarRange = false;

    public void Reset()
    {
        currentWaypoint = 0;
    }

//    public void calculateProximity(Transform subject, TrialWaypoint[] waypoints)
//    {
//        //incrementWaypoint(subject, waypoints, maxSqrDistance);

//        // Get closest point on this line
//        pathPoint = FindClosestPointOnLineSegment(waypoints[currentWaypoint].transform.position, waypoints[currentWaypoint + 1].transform.position, SardineSwim.playerTransform.position);
//        float distanceSqr = (pathPoint - subject.position).sqrMagnitude;

//        // Get the closest point on the NEXT line
//        if (currentWaypoint < waypoints.Length - 2)
//        {
//            Vector3 nextLinePoint = FindClosestPointOnLineSegment(waypoints[currentWaypoint + 1].transform.position, waypoints[currentWaypoint + 2].transform.position, SardineSwim.playerTransform.position);
//            float newDistanceSqr = (nextLinePoint - subject.position).sqrMagnitude;

//            // Keep going, so long as the new point is close enough.
//            while (newDistanceSqr < distanceSqr && newDistanceSqr <= innerRadius * innerRadius && currentWaypoint < waypoints.Length - 2)
//            {
//                pathPoint = nextLinePoint;
//                distanceSqr = newDistanceSqr;
//                currentWaypoint++;

//                nextLinePoint = FindClosestPointOnLineSegment(waypoints[currentWaypoint + 1].transform.position, waypoints[currentWaypoint + 2].transform.position, SardineSwim.playerTransform.position);
//                newDistanceSqr = (nextLinePoint - subject.position).sqrMagnitude;
//            }
//        }


//        // Get player (squared) distance
//        playerCorrection = pathPoint - subject.position;
//        isInCloseRange = distanceSqr < innerRadius * innerRadius;
//        isInFarRange = distanceSqr < outerRadius * outerRadius;

//        if (!isInFarRange)
//        {
//#if UNITY_EDITOR
//            EditorApplication.isPaused = true;
//#endif
//        }
//    }

    // If that's the end of this line, we're ready for the next line (This shouldn't get stuck an in infitite loop, so long as the path isn't a looping circle for some reason)
    //while (pathPoint == waypoints[currentWaypoint + 1].transform.position && currentWaypoint < waypoints.Length - 1)
    //{
    //    currentWaypoint++;
    //    pathPoint = FindClosestPointOnLineSegment(waypoints[currentWaypoint].transform.position, waypoints[currentWaypoint + 1].transform.position, SardineSwim.playerTransform.position);
    //}


    // Find closest point in next line
    // while that point is closer than the current point:
    // make the new point the current point
    // increment the waypoint

    //public float getPlayerPathSqrDistance(Transform subject, TrialWaypoint[] waypoints)
    //{

    //    float distance;
    //    if (currentWaypoint < waypoints.Length - 1)  // If we're not at the last waypoint yet
    //    {
    //        closestPoint = FindClosestPointOnLineSegment(waypoints[currentWaypoint].transform.position, waypoints[currentWaypoint + 1].transform.position, SardineSwim.playerTransform.position);
    //        Debug.Log("POINT: " + closestPoint);//
    //        distance = Mathf.Abs((SardineSwim.playerTransform.position - closestPoint).sqrMagnitude);
    //    }
    //    else  // We must be at the last waypoint
    //    {
    //        distance = Mathf.Abs((SardineSwim.playerTransform.position - waypoints[currentWaypoint].transform.position).sqrMagnitude);
    //    }
    //    Debug.Log("Player path distance: " + distance);//
    //    return distance;
    //}

    //void incrementWaypoint(Transform subject, TrialWaypoint[] waypoints, float maxSqrDistance)
    //{
    //    if (currentWaypoint < waypoints.Length - 1 && Vector3.Distance(subject.position, waypoints[currentWaypoint+1].transform.position) <= waypoints[currentWaypoint].distance)
    //        currentWaypoint++;


    //}

    private static Vector3 FindClosestPointOnLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 line = lineEnd - lineStart;
        Vector3 dir = point - lineStart;
        float d = Vector3.Dot(line, dir) / line.sqrMagnitude;
        d = Mathf.Clamp01(d);
        return Vector3.Lerp(lineStart, lineEnd, d);
    } // https://answers.unity.com/questions/1201367/finding-distance-of-player-to-a-path-in-3d.html

    private void OnDrawGizmos()
    {
        if (pathPoint == null)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawCube(pathPoint, Vector3.one * 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pathPoint, outerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pathPoint, innerRadius);

    }

}
