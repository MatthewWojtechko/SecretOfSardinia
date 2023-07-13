using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Chasing : PhysicalState<EnemyStateMachine>
{
    private bool eluded = false;
    public override void enter(EnemyStateMachine machine)
    {
        eluded = false;
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        // move!
    }

    public override void update(EnemyStateMachine machine)
    {
        // Have they eluded us? Have we run into a light?
    }

    protected override void exit(EnemyStateMachine machine)
    {
        if (eluded)
            machine.enterState(machine.CHASEEND);
        else
            machine.enterState(machine.ESCAPE);
    }
}
