namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TravellingState : BaseState
    {
        public Vector3 targetPosition;
        public override void enterState(StateManager trial)
        {
            //trial.sounds.stopTicking();
            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.STAY;

            trial.waypointManager.beginTripToNextHaltpoint();
            targetPosition = trial.waypointManager.getNextPosition();
            trial.Move.setTarget(targetPosition);
        }

        public override void playerInRadius(StateManager trial)
        {
            if (trial.waypointManager.getHalt().placement == HaltPoint.Standing.LAST)
            {
                trial.switchState(trial.winState);
            }
            else
            {
                trial.switchState(trial.travelState);
            }
        }

        public override void updateState(StateManager trial)
        {
            if (trial.Move.go(trial.speed))  // reached waypoint?
            {
                if (trial.waypointManager.hasReachedWaypoint())  // reached halt?
                {
                    trial.switchState(trial.haltState);
                    return;
                }
                targetPosition = trial.waypointManager.getNextPosition();
                trial.Move.setTarget(targetPosition);
            }

            // Move forward based on speed
            // check if we've reached position
            // if so, check if the position is a halt position, then halt
            // else, pick next position, and turn toward that
            // Regardless, make sure we center on the current point
        }
    }
}