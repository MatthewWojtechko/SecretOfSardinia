using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/*
 * Determines if and how well the player is seen by this enemy.
 */
public class EnemyEyes : MonoBehaviour
{
    public enum State { UNWITTING, SUS, SPOTTED };
    public State currentState = State.UNWITTING;


    public float damage = 1;

    public Transform eyes;
    public Transform noticePoint;
    public Vector3 noticeHalfExtents;
    public float warningFactor = 0.2f;
    private float warningStopwatch = 0;

    public float FOVangle;
    public float sightDistanceUnspotted = 15;
    public float sightDistanceSpotted = 20;
    public float currentSightDistance;
    public int forwardMultiplier = -1;

    public LayerMask unawareSightMask;
    public LayerMask awareSightMask;
    private RaycastHit hit = new RaycastHit();
    private Bounds noticeBounds;

    public bool playerInRange = false;

    private int currentSightMask;
    public bool spotted = false;
    private bool suspicious = false;

    public bool stopwatchRunning = true;
    public float stopwatchGoal;

    private float playerDistance;

    private void Start()
    {
        noticeBounds = new Bounds();
        noticeBounds.extents = noticeHalfExtents;
    }

    //private bool seen(bool aware)
    //{
    //    float sightDistance;
    //    if (aware)
    //        sightDistance = sightDistanceSpotted;
    //    else
    //        sightDistance = sightDistanceUnspotted;

    //    Vector3 direction = SardineSwim.playerTransform.position - eyes.position;
    //    playerDistance = direction.magnitude;

    //    if (playerDistance < currentSightDistance)   // If the player is close enough
    //    {
    //        //Debug.Log("PLAYER CLOSE ENOUGH");//
    //        if (Vector3.Angle(direction.normalized, this.transform.forward * forwardMultiplier) < FOVangle || playerInSphere())  // Player within field of view angle
    //        {
    //            // Sight mask differs if spot - if not spotted, more generous hiding
    //            if (aware)
    //                currentSightMask = awareSightMask;
    //            else
    //                currentSightMask = unawareSightMask;

    //            //Debug.Log("PLAYER WITHIN FOV");//

    //            return !Physics.Linecast(eyes.position, SardineSwim.playerTransform.position, currentSightMask);
    //        }
    //    }
    //    return false;
    //}

    public bool seen(bool aware)
    {
        return playerNearInRange(aware) && !playerObstructed(aware);
    }

    public bool playerNearInRange(bool aware)
    {
        float sightDistance;
        if (aware)
            sightDistance = sightDistanceSpotted;
        else
            sightDistance = sightDistanceUnspotted;

        Vector3 direction = SardineSwim.playerTransform.position - eyes.position;
        playerDistance = direction.magnitude;

        if (playerDistance < currentSightDistance)   // If the player is close enough
        {
            //Debug.Log("PLAYER CLOSE ENOUGH");//
            return (Vector3.Angle(direction.normalized, this.transform.forward * forwardMultiplier) < FOVangle || playerInSphere()); // Player within field of view angle
        }
        return false;
    }

    public bool playerObstructed(bool aware)
    {
        // Sight mask differs if spot - if not spotted, more generous hiding
        if (aware)
            currentSightMask = awareSightMask;
        else
            currentSightMask = unawareSightMask;

        //Debug.Log("PLAYER WITHIN FOV");//

        return Physics.Linecast(eyes.position, SardineSwim.playerTransform.position, currentSightMask);
    }

    public float getWarningTime()
    {
        return warningFactor * playerDistance;
    }

    //public void giveDamage()
    //{
    //    PlayerHealth.seen(Time.deltaTime * damage);
    //}

    public float getPlayerDistance()
    {
        return playerDistance;
    }


    bool playerInSphere()
    {
        noticeBounds.center = noticePoint.position;
        return noticeBounds.Contains(SardineSwim.playerTransform.position); 
    }

    // https://stackoverflow.com/questions/52130986/can-we-create-a-gizmos-like-cone-in-unity-with-script
    void OnDrawGizmosSelected()
    {
        // First outline
        float angle = FOVangle;
        if (Application.isEditor)
            currentSightDistance = sightDistanceUnspotted;
        float rayRange = currentSightDistance;
        float coneDirection = 0;
        float legLength = 0;

        Quaternion upRayRotation = Quaternion.AngleAxis(-angle + coneDirection, Vector3.right);
        Quaternion downRayRotation = Quaternion.AngleAxis(angle + coneDirection, Vector3.right);

        Vector3 upRayDirection = upRayRotation * transform.forward * forwardMultiplier * rayRange;
        Vector3 downRayDirection = downRayRotation * transform.forward * forwardMultiplier * rayRange;

        Gizmos.DrawRay(eyes.position, upRayDirection);
        Gizmos.DrawRay(eyes.position, downRayDirection);
        Gizmos.DrawLine(eyes.position + (downRayDirection * 1.2f), eyes.position + (upRayDirection * 1.2f));



        // Second outline
        //coneDirection = 90;

        upRayRotation = Quaternion.AngleAxis(-angle + coneDirection, Vector3.up);
        downRayRotation = Quaternion.AngleAxis(angle + coneDirection, Vector3.up);

        upRayDirection = upRayRotation * transform.forward * forwardMultiplier * rayRange;
        downRayDirection = downRayRotation * transform.forward * forwardMultiplier * rayRange;

        Gizmos.DrawRay(eyes.position, upRayDirection);
        Gizmos.DrawRay(eyes.position, downRayDirection);
        Gizmos.DrawLine(eyes.position + downRayDirection, eyes.position + upRayDirection);

        float length = Mathf.Abs((eyes.position + downRayDirection).x - (eyes.position + upRayDirection).x);
        Gizmos.DrawWireCube(eyes.position + this.transform.forward * forwardMultiplier * rayRange, new Vector3(length, length, 0.1f));
    }
}
