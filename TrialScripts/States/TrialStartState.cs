namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StartState : BaseState
    {
        public override void enterState(StateManager trial)
        {
            //trial.sounds.startMove();

            // Pulse effect!
            Debug.Log("START");//
            trial.waypointManager.Reset();
            //trial.bubbleLook.shrink();
            //trial.movement.beginMove();

            //trial.stopwatch.begin();

            //trial.bubbleDrift.enabled = false;
            //trial.bubbleDrift.transform.localPosition = Vector3.zero;

            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.STAY;
            trial.switchState(trial.travelState);
        }

        public override void playerInRadius(StateManager trial)
        {
        }

        public override void updateState(StateManager trial)
        {
        }
    }
}
