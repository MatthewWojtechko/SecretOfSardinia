using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : EnemyState
{
    public override void enter(EnemyStateMachine machine)
    {
        machine.Controller.Animation.attack();
        // play attack animation
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.CHASE);
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        //
    }

    public override void update(EnemyStateMachine machine)
    {
        //
    }
}
