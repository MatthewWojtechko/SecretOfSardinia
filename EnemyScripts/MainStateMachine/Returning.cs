using Pathing;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Returning : EnemyState
{
    bool isSus = false;
    public override void enter(EnemyStateMachine machine)
    {
        //Debug.Log("RETURNING!");///
        isSus = false;
        //machine.Controller.Animation.swim();
        machine.Controller.Pather.homecoming();
    }

    protected override void exit(EnemyStateMachine machine)
    {
        if (isSus)
            machine.enterState(machine.INVESTIGATE);
        else
        {
            machine.Controller.Pather.returned();
            if (machine.species == Breed.HIDER)  // Reached the goal
                machine.enterState(machine.HIDE);
            else
                machine.enterState(machine.PATROL);
        }
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        machine.Controller.Movement.moveTo(machine.Controller.Pather.getNextReturnPoint(), machine.Controller.Movement.DEFAULT_SPEED);
    }

    public override void update(EnemyStateMachine machine)
    {
        // Have we reached the next point?
        if (machine.Controller.Movement.isAtGoal(machine.Controller.Pather.getNextReturnPoint()))  
        {
            machine.Controller.Pather.reachedNextReturnPoint();
            if (machine.Controller.Pather.areWeBackYet())  
            {
                isSus = false;
                exit(machine);  // We made it back!
            }
        }

        // if we are suspicious
        if (machine.Controller.Suspicion.currentState is Noticing)
        {
            isSus = true;
            exit(machine);
        }
    }
}
