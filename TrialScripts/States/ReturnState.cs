namespace WaveTrial
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ReturnState : BaseState
    {
        float waitTime = 2;
        float timeEnterState;
        public override void enterState(StateManager trial)
        {
            //trial.bubbleLook.shrink();
            timeEnterState = Time.time;
            trial.waypointManager.Reset();
        }

        public override void playerInRadius(StateManager trial)
        {
        }

        public override void updateState(StateManager trial)
        {
            if (Time.time - timeEnterState > waitTime)
            {
                trial.transform.position = trial.startPos;
                //trial.bubbleLook.expand();
                trial.switchState(trial.idleState);
            }
        }
    }
}
