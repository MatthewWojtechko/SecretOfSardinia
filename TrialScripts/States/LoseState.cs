namespace WaveTrial
{
    using UnityEngine;

    public class LoseState : BaseState
    {
        float waitAfterLoss = 1;
        float timeLost;
        public override void enterState(StateManager trial)
        {
            //// Lose - trail disappears
            //trial.sounds.failure();
            //trial.sounds.stopTicking();
            //trial.sounds.stopTicking();
            //TrialWarningPart.stop();


            //trial.timeOutBounds = -1; // reset
            //trial.transform.position = trial.startPos;
            //trial.bubbleLook.expand();
            //TrialWarningPart.freeze();
            //timeLost = Time.time;

            //trial.stopwatch.end();
            //trial.sounds.stopMove();

            //TrialArrow.stopPoint();
            trial.SpiritMaster.currentState = SpiritMovement.SpiritState.STAY;
        }

        public override void playerInRadius(StateManager trial)
        {
        }

        public override void updateState(StateManager trial)
        {
            if (Time.time - timeLost > waitAfterLoss)
            {
                TrialWarningPart.stop();
                trial.switchState(trial.returnState);
            }
        }
    }
}