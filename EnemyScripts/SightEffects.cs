using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightEffects
{
    EnemyController Controller;
    bool lineNeedsUpdated = false;

    public SightEffects(EnemyController controller)
    {
        Controller = controller;
    }


    public void UpdateCode()
    {
        if (Controller.Suspicion.currentState is Seeing)
        {
            if (Controller.Behavior.currentState == Controller.Behavior.CHASE)
            {
                attack();
            }
            else
            {
                warn();
            }
        }
        else if (Controller.Suspicion.currentState is Curious && Controller.Knowledge.isPlayerVisible())
        {
            warn();
        }
        else //if (Suspicion.currentState is Eluded || Suspicion.currentState is Unwitting)
        {
            lineNeedsUpdated = false;
            Controller.LineManager.lostSight();
        }

        void warn()
        {
            Controller.LineManager.warning(Controller.Knowledge.getSeeStopwatch(), Controller.Knowledge.getSightThreshold());
            lineNeedsUpdated = true;
        }

        void attack()
        {
            lineNeedsUpdated = true;
            Controller.LineManager.attack(Mathf.Clamp01(Controller.Look.playerDistancePercent_distanceCached()));
        }
    }

    public void FixedUpdateCode()
    {
        updateLine();
    }

    public void updateLine()
    {
        if (lineNeedsUpdated)
        {
            if (Controller.Knowledge.getIsPlayerUnobstructed())
            {
                Controller.LineManager.on();
                Controller.LineManager.updatePosition();
            }
            else
            {
                Controller.LineManager.off();
            }
        }
    }
}
