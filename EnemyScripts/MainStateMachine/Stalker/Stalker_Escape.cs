using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Evade : PhysicalState<EnemyStateMachine>
{
    public override void enter(EnemyStateMachine machine)
    {
        //
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        // Escape the light we're in - go backwards until we're out

        // If we're waiting for the light ahead of us to die, do nothing here.
    }

    public override void update(EnemyStateMachine machine)
    {
        // Are we escaping? Or just waiting?
        // If the coast is clear, then:
        // exit(machine);
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.FOLLOW);
    }
}
