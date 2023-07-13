/*
 * This script looks at where the player is within their line of sight, and factors in some person-like considerations:
 * When you see someone, and they go out of view briefly, you still have confidence where they are.
 * When you think you maaaybe see someone, after so long of not seeing them, you doubt yourself.
 * This script informs the Curious and Seeing state scripts. It also informs scripts that make the enemy move when in pursuit of 
 * the player - specifically, it tells them which way to go. Do they go toward the position of the player or (when the player is  
 * out of sight) to the position they last saw the player at.
 * 
 * This script also controls the sight line. It tells the sight line manager when it should be warning and when it should be attacking, 
 * along with passing some data it needs, like how far the player is and how long they've been in warning.
 */

/*
 *                                                  TO DO:
 * Add a "persistence" toggle.
 * Just because we lost them doesn't mean they eluded us.
 * Keep looking, until we reach the point we last saw the player. Then, if the player still isn't see, we return ELUDE.
 * This will require communcation back from the chasing state code. If the enemy is marked as persistent, it will have to tell 
 * this script if it's reached its destination.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightKnowledge : MonoBehaviour
{
    public EnemyLook Look;
    Status currentStatus; public Status getStatus() { return currentStatus; }
    
    public float curiosityTimeConstant = 5; // This value is used in the equation that determiens how long it takes to find the player when curious.
    public float distanceWeight = 0.5f;
    public float angleWeight = 0.5f;
    private float currentCurioistyTimeThreshold;     // The result of the equation, given the ever-changing player distance and angle, and the constant above.
    public float getSightThreshold() { return currentCurioistyTimeThreshold; }
    float seeStopwatch = 0;                          // How many seconds have we seen the player while curious? When it exceeds the above variable, we know we see them.

    public AnimationCurve distanceWeightCurve;
    public AnimationCurve angleWeightCurve;

    public float getSeeStopwatch()
    {
        return seeStopwatch;
    }

    // How long the player can leave enemy view without the enemy doubting itself.
    float elusionStopwatch = 0;
    [SerializeField] float maxElusionTime = 2;

    // When turning, don't want to doubt until after we've turned around.
    bool needToWaitLonger = false;
    float extraWaitTime = 5;
    float extraWaitStopwatch = 0;

    Vector3 playerLastSeenHere;
    bool isPlayerInRange;
    bool isPlayerUnobstructed; public bool getIsPlayerUnobstructed() { return isPlayerUnobstructed;  }
    public bool isPlayerVisible()
    {
        return isPlayerUnobstructed && isPlayerInRange;
    }

    public Vector3 getPlayerLastPos()
    {
        return playerLastSeenHere;
    }

    void calculateCuriosityDetectTime()
    {
        currentCurioistyTimeThreshold = (distanceWeightCurve.Evaluate((Mathf.Clamp01((Look.getPlayerDistanceSqr() / Mathf.Pow(Look.getCurrentSightDistance(), 2)))) * distanceWeight) + 
                                        (angleWeightCurve.Evaluate((Look.getPlayerAngle() / 360) * angleWeight))) * (curiosityTimeConstant);
    }

    public void beginKnowing()
    {
        seeStopwatch = 0;
        elusionStopwatch = 0;
        extraWaitStopwatch = 0;
    }

    public void beginCuriosity(bool needsExtraTime)
    {
        seeStopwatch = 0;
        elusionStopwatch = 0;
        extraWaitStopwatch = 0;

        needToWaitLonger = needsExtraTime;
    }

    public Status knowingLook()
    {
        checkVisual();

        if (isPlayerInRange && isPlayerUnobstructed)
        {
            playerLastSeenHere = SardineSwim.playerTransform.position;
            elusionStopwatch = 0;
        }
        else
        {
            elusionStopwatch += TimeKeeper.deltaPlayTime();
            if (elusionStopwatch > maxElusionTime)
            {
                currentStatus = Status.ELUDED;
                return Status.ELUDED;
            }
        }

        currentStatus = Status.SEEING;
        return Status.SEEING;
    }

    public Status curiousLook()
    {
        checkVisual();
        //Debug.Log("is player visible: " + isPlayerVisible);//
        //Debug.Log("see: " + seeStopwatch + "\nelusion: " + elusionStopwatch);//

        if (isPlayerInRange && isPlayerUnobstructed)        // i c u
        {
            playerLastSeenHere = SardineSwim.playerTransform.position;
            seeStopwatch += TimeKeeper.deltaPlayTime(); // Count how long player in view.
        }
        else      // where r u
        {
            if (needToWaitLonger)  // We must still be turning, so don't give up for a few extra seconds.
            {
                extraWaitStopwatch += TimeKeeper.deltaPlayTime();
                if (extraWaitStopwatch >= extraWaitTime)
                    needToWaitLonger = false;
            }
            else  // Not turning. Count how long we can't see player.
            {
                elusionStopwatch += TimeKeeper.deltaPlayTime();
            }
        }


        // If we've gone long enough seeing the player, then switch to the SEEING state!
        // If we've gone too long NOT seeing the player, then the player ELUDED us.
        calculateCuriosityDetectTime();
        //Debug.Log("Duration = " + currentCurioistyTimeThreshold);
        if (seeStopwatch > currentCurioistyTimeThreshold)
        {
            currentStatus = Status.FOUND;
            return Status.FOUND;
        }
        if (elusionStopwatch > maxElusionTime)
        {
            currentStatus = Status.ELUDED;
            return Status.ELUDED;
        }

        currentStatus = Status.SEEING;
        return Status.SEEING;
    }

    void checkVisual()
    {
        isPlayerInRange = Look.isPlayerInSight_Calculate();
        isPlayerUnobstructed = Look.isPlayerUnobstructed();
    }

    public bool isPlayerInView()
    {
        return isPlayerInRange && isPlayerUnobstructed;
    }

    public enum Status { SEEING, FOUND, ELUDED };
}
