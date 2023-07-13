using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startled : EnemyState
{
    public float duration = 0.1f;
    public float stopwatch = 0;

    public override void enter(EnemyStateMachine machine)
    {
        stopwatch = 0;
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        //
    }

    public override void update(EnemyStateMachine machine)
    {
        stopwatch += TimeKeeper.deltaPlayTime();
        if (stopwatch > duration)
            exit(machine);
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.INVESTIGATE);
    }
}
