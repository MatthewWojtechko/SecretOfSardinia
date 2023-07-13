namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TerminusState : BaseState
    {
        public override void enterState(StateManager trial)
        {
            trial.sounds.stopTicking();
            trial.bubbleDrift.enabled = false;
            //Debug.Log("END");//
        }

        public override void playerInRadius(StateManager trial)
        {
            trial.switchState(trial.winState);
        }

        public override void updateState(StateManager trial)
        {
            checkPathProxThenOtherStates(trial, warningState);
        }

        void warningState(StateManager trial)
        {
            //if (trial.movement.calculatePlayerTrialDistance(trial.transform) > trial.playerTooFarSqrt)  // In range. Normal end state.
            //    trial.switchState(trial.endWarningState);
        }
    }
}