/*
 * This script creates patrolling behavior by supplying waypoints to travel around, scanning at any waypoints, and wait times at any.
 * Waypoint Navigation works like this: The NPC travels to the waypoint. Once it reaches it, it checks for any 
 * additional behavior. First, it checks if there are any waits supplied for this waypoint. 
 * If so, it will wait the given time. Then, it checks for any scans at that waypoint. If so, it will turn in 
 * the given direction. The NPC goes wait, scan, wait scan, etc. until there are none left to do.
 * Once this behavior is over, it proceeds to the next waypoint. When it reaches the last waypoint in the list, 
 * it either (1) loops back around to the beginning or (2) moves backwards through the waypoints (not implemented 
 * yet).
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public NPCMovement Movement;


    [System.Serializable]
    public enum MovementMode { POINT, WAYPOINT, SCAN, STOP, WAYPOINT_FOLLOW };
    public enum WaypointMode { LOOP, REVERSE };
    private int direction = 1;
    public WaypointMode loopMode = WaypointMode.LOOP;

    public MovementMode currentMode = MovementMode.WAYPOINT;

    //public Transform[] waypoints;
    public Transform waypoints;
    //public WaypointBehavior[] behaviorsAtWaypoints;
    public WaypointBehavior currentBehavior;

    public int currentWaypoint = 0;
    public int behaviorItemIndex;
    public int maxBehaviorItems;
    public bool isWaiting;
    public bool waitTimerStarted = false;


    public bool doingBehavior = false;

    private int waits, scans;


    int searchIndex = 0;
    bool searchStarted = false;
    int searchBestIndex;
    float searchClosestDistance;

    public bool targetClosestWaypoint()
    {
        // The first item in the search. The first item is the de-facto closest, so far.
        if (!searchStarted)
        {
            searchStarted = true;
            searchIndex = 0;
            searchClosestDistance = (waypoints.GetChild(searchIndex).position - this.transform.position).sqrMagnitude;
            searchBestIndex = searchIndex;
            searchIndex++;
            return false;
        }

        // If we've reached the final waypoint, then the best one we have is truly the best.
        if (searchIndex >= maxBehaviorItems)
        {
            currentWaypoint = searchBestIndex;
            return true;
        }

        // Get the distance to the new waypoint
        // If it's closer than the previous clsoest, set that as the current best option.
        float newDistance = (waypoints.GetChild(searchIndex).position - this.transform.position).sqrMagnitude;
        if (newDistance < searchClosestDistance)
        {
            searchClosestDistance = newDistance;
            searchBestIndex = searchIndex;
        }
        return false;
    }

    public void waypointMovement()
    {
        if (Movement.isAtGoal(targetPosition())) // If at the target waypoint
        {
            determineActionAtWaypoint();
        }

        if (!doingBehavior)
        {
            Movement.moveTo(targetPosition(), Movement.DEFAULT_SPEED);  // If not performing behavior, move to the next waypoint
        }
        else
        {
            //Debug.Log("B");//
            performBehavior();
        }
    }

    Vector3 targetPosition()
    {
        try
        {
            return waypoints.GetChild(currentWaypoint).position;
        }
        catch (Exception e)
        {
            Debug.Log("Error! Enemy " + name + " could not access waypoint " + currentWaypoint);//
            return Vector3.zero;
        }
    }

    void performBehavior()
    {
        // No behavior. Skip.
        if (currentBehavior == null)
        {
            doingBehavior = false;
            currentWaypoint = incrementIndex(currentWaypoint);
            return;
        }

        if (isWaiting)
        {
            wait();
        }
        else
        {
            scan();
        }

        // If they've both been exceeded, we're done doing this behavior
        if (behaviorItemIndex > currentBehavior.waitTimes.Length - 1 && behaviorItemIndex > currentBehavior.scanRotations.Length - 1)
        {
            doingBehavior = false;
            currentWaypoint = incrementIndex(currentWaypoint);
        }
    }

    void wait()
    {
        if (behaviorItemIndex <= currentBehavior.waitTimes.Length - 1)  // If there's another wait
        {
            if (!waitTimerStarted)
                StartCoroutine(waitSeconds(currentBehavior.waitTimes[behaviorItemIndex]));
        }
        else
        {
            isWaiting = false; // Otherwise, switch to scanning
        }
    }

    IEnumerator waitSeconds(float sec)
    {
        waitTimerStarted = true;
        isWaiting = true;
        yield return new WaitForSeconds(sec);
        isWaiting = false;
    }

    void determineActionAtWaypoint()
    {
        if (!doingBehavior) // If we're not currently performing waypoint behavior, check to see if we should be.
        {
            //Debug.Log("A");//

            currentBehavior = waypoints.GetChild(currentWaypoint).GetComponent<WaypointBehavior>();
            if (currentBehavior != null) // We've found a behavior.
            {
                behaviorItemIndex = 0;
                doingBehavior = true;
                isWaiting = true;
                waitTimerStarted = false;

                waits = currentBehavior.waitTimes.Length;
                scans = currentBehavior.scanRotations.Length;
                if (waits > scans)
                    maxBehaviorItems = waits;
                else
                    maxBehaviorItems = scans;
            }
            else
            {
                currentWaypoint = incrementIndex(currentWaypoint); // If there wasn't a behavior record for this waypoint, then get ready to move to the next waypoint
            }
        }
    }

    void scan()
    {
        waitTimerStarted = false;
        if (behaviorItemIndex <= currentBehavior.scanRotations.Length - 1)  // If there's another scan
        {
            // Rotate
            Vector3 scanTargetRotation = currentBehavior.scanRotations[behaviorItemIndex];
            Movement.rotateTowardsRot(scanTargetRotation, Movement.DEFAULT_SPEED);

            // Are we done yet?
            if (Movement.scanComplete(scanTargetRotation))
            {
                isWaiting = true;
                behaviorItemIndex++;
            }
        }
        else
        {
            isWaiting = true;
            behaviorItemIndex++;
        }
    }

    public int incrementIndex(int index)
    {
        if (loopMode == WaypointMode.LOOP)  // Loop Mode means go through waypoints like: 1, 2, 3, 1, 2, 3, 1, 2, 3
        {
            if (index >= waypoints.childCount - 1)
                index = 0;
            else
                index++;
        }
        else    // Reverse Mode means go through waypoints like: 1, 2, 3, 2, 1, 2, 3, 2, 1
        {
            index += direction;
            if (index == waypoints.childCount)
            {
                direction = -1;
                index = waypoints.childCount - 1;
            }
            else if (index == -1)
            {
                direction = 1;
                index = 1;

            }
        }

        return index;
    }
}