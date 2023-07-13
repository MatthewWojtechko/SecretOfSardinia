using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noticing : UpdatableState<SuspicionStateMachine>
{
    //public float duration = 1;
    //public float stopwatch = 0;
    public override void enter(SuspicionStateMachine machine)
    {
        //stopwatch = 0;
        // Stop moving. Make a sound/effect (shake?)
    }

    protected override void exit(SuspicionStateMachine machine)
    {
        machine.enterState(machine.Curious);
    }

    public override void update(SuspicionStateMachine machine)
    {
        // Update Line effect ///////////////// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        // If we're physically moving, start gauging our curiosity. Is someone there?
        if (machine.Controller.Behavior.getCurrentState() is Investigating)
        {
            exit(machine);
        }
        //// Wait in place a second.
        //stopwatch += TimeKeeper.deltaPlayTime();
        //if (stopwatch >= duration)
        //    exit(machine);
    }
}
