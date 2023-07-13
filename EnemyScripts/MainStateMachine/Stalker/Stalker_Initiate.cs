using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Initiate : PhysicalState<EnemyStateMachine>
{
    public override void enter(EnemyStateMachine machine)
    {
        // attack animation!
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        // keep moving toward player
    }

    public override void update(EnemyStateMachine machine)
    {
        // when the animation (or duration) is over, exit
    }

    protected override void exit(EnemyStateMachine machine)
    {
        machine.enterState(machine.CHASE);
    }
}
