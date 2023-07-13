using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnd : EnemyState
{
    float duration = 1;
    float stopwatch;
    public override void enter(EnemyStateMachine machine)
    {
        stopwatch = 0;
        // play idle animation
        machine.Controller.Animation.idle();
        // TO DO:
        // if dead, play theatening animation (alarm)
        // if alive, play confused animation
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.RETURN);
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        //
    }

    public override void update(EnemyStateMachine machine)
    {
        stopwatch += TimeKeeper.deltaPlayTime();
        if (stopwatch > duration)
        {
            exit(machine);
        }
    }
}
