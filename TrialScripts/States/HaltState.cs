namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HaltState : BaseState
    {
        public override void enterState(StateManager trial)
        {
            // Effect!

            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.CIRCLE;
            trial.timer.BeginImmediately();
        }

        public override void playerInRadius(StateManager trial)
        {
            // TODO: Even when the trial is still in the travelling state, if you reach its halt point, it automatically switches to the next waypoint
            if (trial.waypointManager.getHalt().placement == HaltPoint.Standing.LAST)
            {
                trial.timer.End();
                trial.switchState(trial.winState);
            }
            else
            {
                trial.timer.End();
                trial.switchState(trial.travelState);
            }
        }

        public override void updateState(StateManager trial)
        {
            // Time ticks down
            // LOST STATE if too late

            if (!trial.timer.isComplete())
                return;


            trial.switchState(trial.loseState);
        }
    }
}
