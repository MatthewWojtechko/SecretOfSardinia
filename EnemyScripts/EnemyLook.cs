/*
 * isPlayerInSight - determines if the player is in view of the enemy.
 * There are also methods to get the distance and angle of the player, which could be used 
 * to glean how /well/ the enemy sees the player. For example, the closer the player, and the less angled 
 * the player's location is, the more obviously visibile they are.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLook : MonoBehaviour
{
    public Transform eyes;
    [SerializeField] private float FOVangle;
    [SerializeField] private float sightDistance = 5;
    public int forwardMultiplier = 1;
    public LayerMask obstacleLayer;

    public float multiplier;


    Vector3 direction;
    [SerializeField] float distancesqr = Mathf.Infinity;
    float angle;

    [SerializeField] bool isInSight;

    float FOVangle_scaled;
    //float sightDistance_scaled;

    public bool caulculateDistanceIndependently = false;

    public void Awake()
    {
        FOVangle_scaled = FOVangle /** multiplier*/;
        //sightDistance_scaled = sightDistance /** multiplier*/;
    }

    public float getCurrentSightDistance()
    {
        return sightDistance;// sightDistance_scaled;
    }

    public float getFOVangle()
    {
        return FOVangle_scaled;
    }

    public bool isPlayerInSight_Calculate()
    {
        direction = SardineSwim.playerTransform.position - eyes.position;
        angle = Vector3.Angle(direction.normalized, this.transform.forward * forwardMultiplier);
        if (caulculateDistanceIndependently)
            distancesqr = direction.magnitude;
        isInSight = distancesqr < sightDistance * sightDistance && Vector3.Angle(direction.normalized, this.transform.forward * forwardMultiplier) < FOVangle_scaled;
        return isInSight;
    }

    public float playerDistancePercent_distanceCached()
    {
        return distancesqr / (sightDistance * sightDistance);
    }

    public bool isPlayerInSight_Cached()
    {
        return isInSight;
    }

    public float getPlayerDistanceSqr()
    {
        return distancesqr;
    }
    public float getPlayerAngle()
    {
        return angle;
    }

    public bool isPlayerUnobstructed()
    {
        return !Physics.Linecast(eyes.position, SardineSwim.playerTransform.position, obstacleLayer);
    }

    /*
     * The distance should only be calculated sparingly.
     * It should only be set by one script: EnemyDistanceTrackerMaster
     */
    public void setPlayerDistanceSqr(float d)
    {
        distancesqr = d;
    }

    // https://stackoverflow.com/questions/52130986/can-we-create-a-gizmos-like-cone-in-unity-with-script
    void OnDrawGizmosSelected()
    {
        // First outline
        float angle = FOVangle /** multiplier*/;
        float rayRange = sightDistance /** multiplier*/;
        float coneDirection = 0;
        //float legLength = 0;

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
