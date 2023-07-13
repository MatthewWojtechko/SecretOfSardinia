using Pathing;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigating : EnemyState
{
    private bool isFound = false;
    private bool isReturning;
    private bool suspiciousFromSense;

    public float duration = 0.5f;
    public float stopwatch = 0;
    public override void enter(EnemyStateMachine machine)
    {
        //Debug.Log("ENTERING investigating state");//
        isFound = false;
        suspiciousFromSense = machine.Controller.Vibes.status != 0;
        isReturning = machine.currentState == machine.RETURN;
        stopwatch = 0;
        //machine.Controller.Animation.swim();
        machine.Controller.Pather.embarking();
    }

    protected override void exit(EnemyStateMachine machine)
    {
        //Debug.Log("exiting investigating state");//
        if (isFound)
            machine.enterState(machine.ALARM);
        else if (machine.species == Breed.PATROLLER)
        {
            if (isReturning)
                machine.enterState(machine.RETURN);
            else
                machine.enterState(machine.PATROL);
        }
        else // Must be a Hider, which only get suspicious during their return trip.
        {
            machine.enterState(machine.RETURN);
        }
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        if (stopwatch < duration)
            return;
        Debug.Log("fd");///

        suspiciousFromSense = machine.Controller.Vibes.status != 0;
        if (machine.Controller.Look.isPlayerInSight_Cached())   // If player is in view, go that way
        {
            //Debug.Log("going to player");//
            machine.Controller.Movement.moveTo(machine.Controller.Knowledge.getPlayerLastPos(), machine.Controller.Movement.SUS_SPEED);
        }
        else if (suspiciousFromSense)   // If player was senses nearby, go to where player last sensed
        {
            //Debug.Log("going to player");//
            machine.Controller.Movement.moveTo(machine.Controller.Vibes.playerLastPosition, machine.Controller.Movement.SUS_SPEED);
            Debug.Log("moving to player");///
        }

        //if (machine.Controller.Look.isPlayerInSight_Cached())   // If player is in view, go that way
        //{
        //    //Debug.Log("going to player");//
        //    machine.Controller.Movement.moveTo(machine.Controller.Knowledge.getPlayerLastPos(), machine.Controller.Movement.DEFAULT_SPEED);
        //}
        //else if (suspiciousFromSense)    // Else, if suspicious because the player was "sensed" nearby, turn in that direction
        //{
        //    //Debug.Log("turning toward sense");//
        //    // turn to player
        //    //Debug.Log(machine.Controller.Sense.getSensedPos() + " " + SardineSwim.playerTransform.position);///
        //    machine.Controller.Movement.lookAt(machine.Controller.Vibes.getSensedPos(), machine.Controller.Movement.SUS_SPEED);
        //}
        // Otherwise, we have no idea. Just chill.
    }

    public override void update(EnemyStateMachine machine)
    {
        //Debug.Log("current sus state: " + machine.Controller.Suspicion.currentState.GetType()); //

        // Wait in place a second.
        stopwatch += TimeKeeper.deltaPlayTime();

        if (machine.Controller.Suspicion.currentState is Eluded)
        {
            isFound = false;
            exit(machine);
        }
        else if (machine.Controller.Suspicion.currentState is Seeing)
        {
            isFound = true;
            exit(machine);
        }
    }
}
