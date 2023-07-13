using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker_Following : PhysicalState<EnemyStateMachine>
{
    public override void enter(EnemyStateMachine machine)
    {
        //
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        // move to the next point in the path
    }

    public override void update(EnemyStateMachine machine)
    {
        // determine if we're at the end, find the next point
    }

    protected override void exit(EnemyStateMachine machine)
    {
        // Initiate or waiting?
    }
}
