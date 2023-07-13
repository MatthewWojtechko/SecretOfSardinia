//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TrialEndWarningState : TrialBaseState
//{
//    public override void enterState(TrialStateManager trial)
//    {
//        trial.sounds.ticking.Stop();
//        if (trial.timeOutBounds < 0)
//            trial.timeOutBounds = Time.time;
//        TrialWarningPart.play();
//        trial.bubbleDrift.enabled = false;
//    }

//    public override void onTriggerEnter(TrialStateManager trial, Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            trial.switchState(trial.winState);
//        }
//    }

//    public override void updateState(TrialStateManager trial)
//    {
//        checkPathProxThenOtherStates(trial, endOrLoseStates);
//    }

//    void endOrLoseStates(TrialStateManager trial)
//    {
//        if (trial.movement.calculatePlayerTrialDistance(trial.transform) <= trial.playerTooFarSqrt)  // In range. Normal end state.
//        {
//            TrialWarningPart.stop();
//            trial.switchState(trial.endState);
//        }
//        else if (Time.time - trial.timeOutBounds > trial.outOfRangeMaxSec) // Out of bounds too long? Lose.
//        {
//            trial.switchState(trial.loseState);
//        }
//    }
//}
