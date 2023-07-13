namespace WaveTrial
{
    using UnityEngine;

    [System.Serializable]
    public class Waypoint : MonoBehaviour
    {
        public bool drawGizmos = true;
        //public float swimSpeed;
        //public float turnSeconds;
        //public float arrivalSqrDistThresh = 0.0625f;

        //public Vector3 normalizedDirection;
        //public float legDistance;
        //public float normalLegDurationSec;
        //public float currentLegDurationSec;
        //public float previousLegsSeconds;
        //public float distanceTravelled = 0;

        public virtual void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(this.transform.position, 0.15f);
        }

        //public void setup(Transform nextWaypoint, float prevLegSeonds)
        //{
        //    if (nextWaypoint == null)
        //        return;
        //    Vector3 moveVector = nextWaypoint.position - this.transform.position;
        //    legDistance = moveVector.magnitude;
        //    normalLegDurationSec = legDistance / swimSpeed;
        //    normalizedDirection = moveVector.normalized;
        //    previousLegsSeconds = prevLegSeonds;
        //    distanceTravelled = 0;
        //}

        //public void beginWaypoint()
        //{
        //    distanceTravelled = 0;
        //}
        //public void beginWaypoint(float headstartDist)
        //{
        //    Debug.Log("Head start distance: " + headstartDist);//
        //    distanceTravelled = headstartDist;
        //}

        //// When given the amount of time the trial has been active, returns where the trial should be by now.
        //// This can be used to move the Bubble over a fixed rate of time.
        //public Vector3 getPosition(float deltaTime, TrialStateManager trial)
        //{
        //    // The new position is the current position, plus a new Vector in the direction that leads to the next waypoint.
        //    // The magnitude of that Vector - AKA the distance that Vector stretches - is proportional to how far we have travelled.

        //    float speed = trial.movement.calculateSpeedIncrease(swimSpeed);
        //    Debug.Log("SPEED: " + speed);//
        //    distanceTravelled += speed * deltaTime;
        //    //Debug.Log(distanceTravelled);
        //    Vector3 newPosition = this.transform.position + (distanceTravelled * normalizedDirection);
        //    //Vector3 newPosition = this.transform.position + (Mathf.Clamp01((totalTrialDuration - previousLegsSeconds) / normalLegDurationSec) * legDistance * normalizedDirection);
        //    return newPosition;
        //}

        ////// When given the amount of time the trial has been active, returns where the trial should be by now.
        ////// This can be used to move the Bubble over a fixed rate of time.
        ////public Vector3 getPosition(float legDurationSoFar)
        ////{
        ////    // The new position is the current position, plus a new Vector in the direction that leads to the next waypoint.
        ////    // The magnitude of that Vector - AKA the distance that Vector stretches - is proportional to how much time we have spent on the current leg.

        ////    Vector3 newPosition = this.transform.position + (Mathf.Clamp01(legDurationSoFar / currentLegDurationSec) * legDistance * normalizedDirection);
        ////    //Vector3 newPosition = this.transform.position + (Mathf.Clamp01((totalTrialDuration - previousLegsSeconds) / normalLegDurationSec) * legDistance * normalizedDirection);
        ////    return newPosition;
        ////}

        //// When given the amount of time the trial has been active, returns whether we are ready for a new waypoint.
        //// If the number is positive, that number represents how much farther we have to go on this waypoint.
        //// If the number is NEGATIVE, that number represents how far we've passed this waypoint.
        //public float getWaypointProgress()
        //{
        //    return legDistance - distanceTravelled;
        //    //return totalTrialDuration - previousLegsSeconds >= normalLegDurationSec;
        //}
    }
}
