namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class WinState : BaseState
    {
        public float winDuration = 12;
        private float winBeginTime;

        public static event Action onTrialNewlyWon;

        // CHANGE what happens when you win
        public override void enterState(StateManager trial)
        {
            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.STAY;
            //trial.sounds.succeed();

            ////////Debug.Log("you win!");//
            ////trial.winPart.Play();
            //winBeginTime = Time.time;

            //float timeScore = trial.stopwatch.end();

            //if (!SaveDataManager.trials.isAlreadyWon(trial.saverLoader.id))  // If this is this trial's first win...
            //{
            //    if (SaveDataManager.trials.win(trial.saverLoader.id, timeScore))  // Inform the Scores class that this trial has been won.
            //    {
            //        // High score!
            //        trial.currentBestScore = timeScore;
            //    }
            //    onTrialNewlyWon?.Invoke();
            //}
            //else if (SaveDataManager.trials.win(trial.saverLoader.id, timeScore))  // Inform the Scores class that this trial has been won.
            //{
            //    trial.bubbleLook.setHasWon();
            //    // High score!
            //    trial.currentBestScore = timeScore;
            //}


            //TrialManager.winTrial(trial.id);
            // cutscene
            // then go back

            trial.switchState(trial.returnState);//
        }

        public override void updateState(StateManager trial)
        {
            //if (Time.time > winBeginTime + winDuration)
            //{
            //    trial.winPart.Stop();
            //    trial.switchState(trial.returnState);
            //}
        }

        public override void playerInRadius(StateManager trial)
        {

        }
    }
}
