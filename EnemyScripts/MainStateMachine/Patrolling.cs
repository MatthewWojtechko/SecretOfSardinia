using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : EnemyState
{
    public override void enter(EnemyStateMachine stateMachine)
    {
        //stateMachine.Controller.Animation.swim();
        // waypoint movement
    }

    protected override void exit(EnemyStateMachine stateMachine)
    {
        stateMachine.enterState(stateMachine.INVESTIGATE);
    }

    public override void fixedUpdate(EnemyStateMachine stateMachine)
    {
        // waypoint moving
        stateMachine.Controller.PatrollerController.waypointMovement();
    }

    public override void update(EnemyStateMachine machine)
    {
        // if we are suspicious...
        if (machine.Controller.Suspicion.currentState is Noticing)
        {
            exit(machine);
        }
    }
}
