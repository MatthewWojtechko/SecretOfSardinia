using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eluded : UpdatableState<SuspicionStateMachine>
{
    public float duration = 1;
    public float stopwatch = 0;
    public override void enter(SuspicionStateMachine machine)
    {
        stopwatch = 0;
        // Animation, sound
    }

    protected override void exit(SuspicionStateMachine machine)
    {
        machine.enterState(machine.Unwitting);
    }

    public override void update(SuspicionStateMachine machine)
    {
        // Wait in place a second.
        stopwatch += TimeKeeper.deltaPlayTime();
        if (stopwatch >= duration)
            exit(machine);
    }
}
