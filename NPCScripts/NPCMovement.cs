using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This script controls basic movement of an NPC - moving toward a specific point or just rotating. 
 */

//[System.Serializable]
//public struct WaypointBehavior
//{
//    public int waypointIndex;           // Which waypoint does this correspond to
//    public float[] waitTimes;           // How long to stay still. 
//    public Vector3[] scanRotations;     // WHich directions to turn toward.
//}

public class NPCMovement : MonoBehaviour
{
    public Rigidbody fishRigid;

    public Speed[] Speeds;
    public int DEFAULT_SPEED = 0, CHASING_SPEED = 1, SUS_SPEED = 2;
    public int currentSpeedType = 0;

    public bool overrideSpeed = false;
    public int forwardMultiplier = 1;

    public float adjustedTargetSpeed;

    public Speed getSpeed()
    {
        return Speeds[currentSpeedType];
    }

    public void rotateTowardsRot(Vector3 targetRotation, int speedType)
    {
        Debug.Log(targetRotation);//
        if (!overrideSpeed)
            currentSpeedType = speedType;
        fishRigid.rotation = Quaternion.Slerp(fishRigid.rotation, Quaternion.Euler(targetRotation), getSpeed().getAngleLerp_stationary());
        // stop once we get there
    }

    public void lookAt(Vector3 targetRotation, int speedType)
    {
        if (!overrideSpeed)
            currentSpeedType = speedType;
        Vector3 idealDirection = (targetRotation - this.transform.position).normalized;
        fishRigid.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(forwardMultiplier * idealDirection, Vector3.up), getSpeed().getAngleLerp_stationary());
    }

    public void moveTo(Vector3 goal, int speedType)
    {
        // If  already at location, exit.
        //if (((goal - this.transform.position).sqrMagnitude <= minDistToGoal * minDistToGoal))
        //    return;
        if (!overrideSpeed)
            currentSpeedType = speedType;

        Vector3 differenceVector = goal - this.transform.position;
        float distance = differenceVector.magnitude;
        Vector3 direction = differenceVector / distance;

        fishRigid.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(forwardMultiplier * direction, Vector3.up), getSpeed().getAngleLerp_waypoint(distance));
        // If not moving at target speed
        adjustedTargetSpeed = getSpeed().getTargetSpeed();
        if (fishRigid.velocity.magnitude < adjustedTargetSpeed * VelocityScale(this.transform.forward * forwardMultiplier, direction))
            fishRigid.AddForce(getSpeed().moveForce * this.transform.forward * forwardMultiplier);
        //Debug.Log(fishRigid.velocity.magnitude);
    }

    public float VelocityScale(Vector3 currentDir, Vector3 idealDir)
    {
        float scale = Vector3.Dot(currentDir, idealDir);
        //Debug.Log(scale);//
        return Mathf.Clamp(scale, 0.0f, 1.0f);
    }

    public bool isAtGoal(Vector3 goal)
    {
        return (goal - this.transform.position).sqrMagnitude < getSpeed().minDistToGoal * getSpeed().minDistToGoal;
    }


    public bool scanComplete(Vector3 targetRotation)
    {
        //Debug.Log("ANGLES: " + Mathf.Abs(Quaternion.Angle(fishRigid.rotation, Quaternion.Euler(targetRotation))));//
        //Debug.Log("CURRENT: " + fishRigid.rotation.eulerAngles);
        return Mathf.Abs(Quaternion.Angle(fishRigid.rotation, Quaternion.Euler(targetRotation))) < getSpeed().minAngleToScan;
    }

    [System.Serializable]
    public class Speed
    {
        public float moveForce;
        public float minimumForce = 1;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float angleLerp;
        public float minDistToGoal = 0.2f;
        public float minAngleToScan = 1;
        public float accurateAngleDistance = 1;
        public EnemySpeedInterpolation interpolation;

        public float getAngleLerp_waypoint(float currentDistance)
        {
            if (accurateAngleDistance == 0 || currentDistance > accurateAngleDistance)
                return angleLerp;
            else
                return Mathf.Lerp(angleLerp, 1, 1 - currentDistance / accurateAngleDistance);
        }
        public float getAngleLerp_stationary()
        {
            return angleLerp;
        }

        public float getTargetSpeed()
        {
            if (interpolation.isActive)
            {
                return interpolation.interpolate(maxSpeed);
            }
            else
            {
                return maxSpeed;
            }
        }

        public float getRawSpeed()
        {
            return maxSpeed;
        }
    }


}
