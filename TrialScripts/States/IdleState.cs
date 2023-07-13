namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IdleState : BaseState
    {
        public override void enterState(StateManager trial)
        {
            //trial.bubbleDrift.enabled = true;
            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.CIRCLE;
        }

        public override void playerInRadius(StateManager trial)
        {
             trial.switchState(trial.startState);
        }

        public override void updateState(StateManager trial)
        {
        }
    }
}
