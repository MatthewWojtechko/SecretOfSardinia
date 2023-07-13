using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curious : UpdatableState<SuspicionStateMachine>
{
    bool foundPlayer = false;
    public override void enter(SuspicionStateMachine machine)
    {
        foundPlayer = false;
        machine.Controller.Knowledge.beginCuriosity(!machine.Controller.Look.isPlayerInSight_Cached());  // If not in sight, we must be turning. Don't give up until after we turn.
    }

    protected override void exit(SuspicionStateMachine machine)
    {
        if (foundPlayer)
            machine.enterState(machine.Seeing);
        else
            machine.enterState(machine.Eluded);
    }

    public override void update(SuspicionStateMachine machine)
    {
        // Look for the player.
        // If we've found them, or lost them, exit to the corresponding state.
        switch (machine.Controller.Knowledge.curiousLook())
        {
            case SightKnowledge.Status.FOUND:
                foundPlayer = true;
                exit(machine);
                break;

            case SightKnowledge.Status.ELUDED:
                foundPlayer = false;
                exit(machine);
                break;
        }
    }

}
