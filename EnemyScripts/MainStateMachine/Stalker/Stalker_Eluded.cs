using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Eluded : PhysicalState<EnemyStateMachine>
{
    float stopwatch = 0;
    public override void enter(EnemyStateMachine machine)
    {
        stopwatch = 0;
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
    }

    public override void update(EnemyStateMachine machine)
    {
        stopwatch += TimeKeeper.deltaPlayTime();
        if (stopwatch > machine.Controller.eludedDuration)
            exit(machine);
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.FOLLOW);
    }
}
