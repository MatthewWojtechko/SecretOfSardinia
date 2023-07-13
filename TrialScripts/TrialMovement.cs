using UnityEngine;
namespace WaveTrial
{
    public class TrialMovement
    {
        //float minPlayerSqrDist = 150;
        //float maxSpeedFactor = 3;
        //float playerSqrDistance;

        //int currentWaypoint = 0;
        //float previousProximity;
        //float rotStartedTime = 0;
        //float waypointProgress = 0;

        //TrialStateManager trial;
        //Transform trialTransform;

        //public TrialMovement(TrialStateManager stM)
        //{
        //    trial = stM;
        //    trialTransform = trial.transform;
        //}

        //public void beginMove()
        //{
        //    currentWaypoint = 0;
        //    resetWaypointFields();
        //    trial.waypoints[0].beginWaypoint();
        //}

        //// Move the trial towards the next waypoint, according to the specifications set by the current waypoint.
        //// These specifications include the rate at which the waypoint should move and the direction.
        //// Returns whether we have made it to the final waypoint or not.
        //public bool move()
        //{
        //    TrialWaypoint wpData = trial.waypoints[currentWaypoint];

        //    // See which waypoint we're at
        //    float waypointProgress = wpData.getWaypointProgress();
        //    while (waypointProgress < 0)  // While we've passed the needed distance for this waypoint
        //    {
        //        Debug.Log(wpData.getWaypointProgress());///
        //        currentWaypoint++;
        //        wpData = trial.waypoints[currentWaypoint];
        //        wpData.beginWaypoint(-waypointProgress);  // Set up the next waypoint, with the extra distance
        //        waypointProgress = wpData.getWaypointProgress();

        //        // If we're at the last waypoint
        //        if (currentWaypoint == trial.waypoints.Length - 1)
        //        {
        //            trialTransform.position = wpData.transform.position;
        //            return true;
        //        }
        //    }

        //    trialTransform.position = wpData.getPosition(TimeKeeper.deltaPlayTime(), trial);
        //    return false;


        //    //float movement = getSpeed(wpData.unitsPerSecond, trial) * TimeKeeper.deltaPlayTime();
        //    //Vector3 newPosition;
        //    //waypointProgress += movement;
        //    //trial.forward = wpData.normalizedDirection;

        //    //if (waypointProgress >= wpData.legDistance)  // If we have arrived, reset, and center on the new waypoint
        //    //{
        //    //    resetWaypointFields();
        //    //    currentWaypoint++;
        //    //    newPosition = allData[currentWaypoint].transform.position;
        //    //    trial.position = newPosition;

        //    //    return (currentWaypoint == allData.Length - 1);
        //    //}
        //    //else
        //    //{
        //    //    newPosition = trial.position + (wpData.unitsPerSecond * TimeKeeper.deltaPlayTime() * wpData.normalizedDirection);
        //    //    trial.position = newPosition;

        //    //    return false;
        //    //}
        //}

        //void resetWaypointFields()
        //{
        //    rotStartedTime = Time.time;
        //    previousProximity = -Mathf.Infinity;
        //    waypointProgress = 0;
        //}

        //// We know the trial has reached the destination if the current distance from it is within the threshold,
        //// or if the current distance is farther than the distance it was previously (meaning it is now overshooting it).
        //bool arrivedWaypoint(Waypoint point, Transform trial)
        //{
        //    Vector3 goal = point.transform.position;
        //    float proximity = (trial.position - goal).sqrMagnitude;
        //    bool result = proximity < point.arrivalSqrDistThresh;
        //    //if (!result)
        //    //    result = proximity > previousProximity;
        //    previousProximity = proximity;
        //    return result;
        //}

        //// Returns what the current speed should be. 
        //// If the player is too close to the player, then a new speed is calculated based on their proximity. The closer the player, 
        //// the faster the speed. This option ensures that the new speed is at least as fast as what the speed would normally be.
        //// Otherwise, if the player is behind enough, then the returned speed is the float passed to the function - the default minimum speed.
        ////float getSpeed(float speed, Transform trial)
        ////{
        ////    float newSpeed = 0;
        ////    playerSqrDistance = (trial.position - SardineSwim.playerTransform.position).sqrMagnitude;
        ////    //Debug.Log("SQR DIST: " + playerSqrDistance);////
        ////    if (playerSqrDistance < minPlayerSqrDist)
        ////    {
        ////        //Debug.Log("TOO CLOSE");//
        ////        float proxFactor = Mathf.Clamp01((minPlayerSqrDist - playerSqrDistance) / minPlayerSqrDist);  // How much of the speed factor to apply - when ontop, we use all of it. When far, use none of it. (I don't think the clamp is needed.)
        ////        float playerSpeed = SardineSwim.playerRigid.velocity.magnitude;
        ////        newSpeed = playerSpeed * proxFactor * maxSpeedFactor;
        ////        if (newSpeed < playerSpeed)
        ////            newSpeed = playerSpeed;
        ////        if (newSpeed < speed)
        ////            newSpeed = speed;
        ////    }
        ////    else
        ////        newSpeed = speed;

        ////    return newSpeed;
        ////}


        //// Determines how much faster than usual the trial should go.
        //// If the player is far from the trial, the factor is 1 -- meaning the trial should go at its current speed.
        //// If the player is really close to the trial, the factor will be greater -- a factor that will make the trial go faster than the player.
        //// NOTE: This function must be called AFTER calculatePlayerTrialDistance, so that the playerSqrDistance value is accurate.
        //public float calculateSpeedIncrease(float normalSpeed)
        //{
        //    float proxFactor = Mathf.Clamp01((minPlayerSqrDist - playerSqrDistance) / minPlayerSqrDist);  // How much of the speed factor to apply - when ontop, we use all of it. When far, use none of it. (I don't think the clamp is needed.)
        //    float newSpeed = proxFactor * SardineSwim.instance.approxSpeed() * trial.maxSpeedRelativeToPlayer;
        //    Debug.Log("Fish Speed: " + SardineSwim.instance.body.velocity.magnitude + " Trial Max Speed: " + SardineSwim.instance.approxSpeed());//
        //    if (newSpeed < normalSpeed)
        //        newSpeed = normalSpeed;
        //    // Welcome back! This seems to not work. Maybe we are calculating the fish's speed incorrectly. And it's always lower than it should be. 
        //    return newSpeed;
        //}

        //public float calculatePlayerTrialDistance(Transform trial)
        //{
        //    playerSqrDistance = (trial.position - SardineSwim.playerTransform.position).sqrMagnitude;
        //    return (trial.position - SardineSwim.playerTransform.position).sqrMagnitude;
        //}

        //// Rotates the given trial's transform toward the target specified in the data.
        //Quaternion getRotation(Waypoint data, Transform trial)
        //{
        //    Vector3 direction = (data.transform.position - trial.position).normalized;
        //    Quaternion lookRot = Quaternion.LookRotation(direction);
        //    float lerp = Mathf.Clamp01((TimeKeeper.getOverworld().getTime() - rotStartedTime) / data.turnSeconds);
        //    return Quaternion.Slerp(trial.rotation, lookRot, lerp);
        //} // https://answers.unity.com/questions/254130/how-do-i-rotate-an-object-towards-a-vector3-point.html
    }
}