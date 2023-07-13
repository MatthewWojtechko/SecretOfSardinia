using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarming : EnemyState
{
    float duration = 1;
    float stopwatch = 0;
    public override void enter(EnemyStateMachine machine)
    {
        machine.Controller.Animation.spotted();
        stopwatch = 0;
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.CHASE);
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        machine.Controller.Movement.lookAt(SardineSwim.playerTransform.position, machine.Controller.Movement.CHASING_SPEED); // Face the player
    }

    public override void update(EnemyStateMachine machine)
    {
        stopwatch += TimeKeeper.deltaPlayTime();
        if (stopwatch > duration)
            exit(machine);
    }
}
