namespace WaveTrial
{
    using UnityEngine;
    using System;

    public abstract class BaseState
    {
        public abstract void enterState(StateManager trial);

        public abstract void updateState(StateManager trial);

        public abstract void playerInRadius(StateManager trial);

        public void checkPathProxThenOtherStates(StateManager trial, Action<StateManager> stateChecks)
        {
            //trial.pathDistance.calculateProximity(SardineSwim.playerTransform, trial.waypoints);  // Player distance from path
            if (!trial.pathDistance.isInFarRange)  // Too far from path
            {
                //Debug.Log("Too far - you lose!");//
                trial.switchState(trial.loseState);
            }
            else
            {
                if (!trial.pathDistance.isInCloseRange)
                {
                    // Play path warning
                    ////Debug.Log("You're getting too far! Move this way: " + trial.pathDistance.playerCorrection);
                    TrialArrow.point(trial.pathDistance.playerCorrection, trial.pathDistance.pathPoint);
                }
                else
                {
                    TrialArrow.stopPoint();
                }

                stateChecks(trial);
            }
        }
    }
}

// https://www.youtube.com/watch?v=Vt8aZDPzRjI