//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TrialMovingWarningState : BaseState
//{
//    public override void enterState(TrialStateManager trial)
//    {
//        trial.sounds.startTicking();
//        trial.timeOutBounds = Time.time;
//        TrialWarningPart.play();
//    }

//    public override void onTriggerEnter(TrialStateManager trial, Collider othe)
//    {
//    }

//    public override void updateState(TrialStateManager trial)
//    {
//        if (trial.movement.move())  // Waypoint movement. If reach end, switch.
//        {
//            trial.sounds.stopMove();
//            trial.bubbleLook.expand();
//            trial.switchState(trial.endWarningState);
//        }

//        checkPathProxThenOtherStates(trial, movingOrLoseStates);
//    }

//    void movingOrLoseStates(TrialStateManager trial)
//    {
//        if (trial.movement.calculatePlayerTrialDistance(trial.transform) <= trial.playerTooFarSqrt)  // Back in bounds, reset time, back to reg. moving state.
//        {
//            trial.timeOutBounds = -1;
//            TrialWarningPart.stop();
//            trial.switchState(trial.movingState);
//        }
//        else if (Time.time - trial.timeOutBounds > trial.outOfRangeMaxSec)  // Else, if time is up, switch to lose.
//        {
//            trial.sounds.stopMove();
//            trial.switchState(trial.loseState);
//        }
//    }
//}
