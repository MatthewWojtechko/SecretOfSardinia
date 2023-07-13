/*
 *  With a thin spherecast, this script checks how much space is above, below, left, and right of the player, up to a specified castDistance.
 *  If there's no collision within the cast distance, the corresponding field is set to Infinity.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    public LayerMask obstacleMask;
    public float radius = 0.2f;
    public float castDistance = 3;
    private static float belowSlopeness = -1; // How vertical the ground beneath Sam is. -1 indicated no ground directly below. 0 indicates horizontal. 1 indicated vertical.
    private static float downSlopeness = -1; 
    private static float slopeThreshold = 0.15f;

    public static float aboveHit;
    public static float belowHit_Local;
    public static float belowHit_World;
    public static float downHit;
    public static float leftHit;
    public static float rightHit;

    //public static bool isSlope = false;

    private int directionIndex = 0;
    RaycastHit hit;

    private void Update()
    {
        setDistance();
        increment();
        //Debug.Log("SLOPNESS: " + slopeness);//
    }

    //private void Update()
    //{
    //    //Debug.Log(slopeness + " > " + slopeThreshold);//
    //    isSlope = slopeness > slopeThreshold;  // distance?
    //}

    public static bool isSlopeBelow()
    {
        return belowSlopeness > slopeThreshold;
    }
    public static bool isSlopeDown()
    {
        return downSlopeness > slopeThreshold;
    }

    void setDistance()
    {
        //if (CreviceCheck.above == 1)  // If we seem to be fairly close to a ceiling, run the above hit check every frame.
        //{
        //    aboveHit = calculate(/*this.transform*/Vector3.up);
        //    if (directionIndex == 0)
        //        return;
        //}
        switch (directionIndex)
        {
            case 0:
                aboveHit = calculate(/*this.transform*/Vector3.up);
                break;

            case 1:
                //belowHit = calculate(this.transform.up * -1);
                belowHit_Local = calculateBelow();
                break;

            case 2:
                leftHit = calculate(this.transform.right * -1);
                break;

            case 3:
                //belowHit = calculate(this.transform.up * -1);
                belowHit_World = calculateDown();
                break;

            default:
                rightHit = calculate(this.transform.right);
                break;

                // Check far below
                // Check far above
                // Make those their own raycast that you only do occasionally, when needed?
        }
    }


    void increment()
    {
        directionIndex++;
        if (directionIndex > 4)
            directionIndex = 0;
    }

    float calculate(Vector3 direction)
    {
        if (SmartPhysics.SphereCast(this.transform.position, radius, direction, out hit, castDistance, obstacleMask))
            return hit.distance; 
        else
            return Mathf.Infinity;
    }

    float calculateBelow()
    {
        if (SmartPhysics.SphereCast(this.transform.position, radius, this.transform.up * -1, out hit, castDistance, obstacleMask))
        {
            belowSlopeness = 1 - hit.normal.y;
            return hit.distance;
        }
        else
        {
            belowSlopeness = -1;
            return Mathf.Infinity;
        }
    }

    float calculateDown()
    {
        if (SmartPhysics.SphereCast(this.transform.position, radius, Vector3.up * -1, out hit, castDistance * 2, obstacleMask))  // Check farther than normal, because GrassCameraController needs to know this
        {
            downSlopeness = 1 - hit.normal.y;
            return hit.distance;
        }
        else
        {
            belowSlopeness = -1;
            return Mathf.Infinity;
        }
    }
}
