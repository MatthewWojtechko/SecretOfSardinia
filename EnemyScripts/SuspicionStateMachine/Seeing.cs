using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeing : UpdatableState<SuspicionStateMachine>
{
    bool playerDied;
    bool isAttacking = false;
    public Seeing()
    {
        PlayerHealth.onPlayerDie += reactToDeath;
    }

    ~Seeing()
    {
        PlayerHealth.onPlayerDie -= reactToDeath;
    }

    void reactToDeath()
    {
        playerDied = true;
    }
    public override void enter(SuspicionStateMachine machine)
    {
        playerDied = false;
        isAttacking = false;

        // Animation, sound
        machine.Controller.Knowledge.beginKnowing();
    }

    protected override void exit(SuspicionStateMachine machine)
    {
        // Stop dealing damage
        if (isAttacking)
            PlayerHealth.endAttack(machine.Controller.damage);

        machine.enterState(machine.Eluded);
    }

    public override void update(SuspicionStateMachine machine)
    {
        if (!isAttacking && machine.Controller.Knowledge.isPlayerInView() && machine.Controller.Behavior.currentState == machine.Controller.Behavior.CHASE)  // We just started chasing, so begin attacking 
        {
            PlayerHealth.beginAttack(machine.Controller.damage);
            isAttacking = true;
        }
        else if (isAttacking && !machine.Controller.Knowledge.isPlayerInView())
        {
            PlayerHealth.endAttack(machine.Controller.damage);
            isAttacking = false;
        }
        if (playerDied)
            exit(machine);

        // Look at the player.

        // If they break our gaze for too long, we lost them. Eluded.
        if (machine.Controller.Knowledge.knowingLook() == SightKnowledge.Status.ELUDED)
            exit(machine);
        // Have they escaped our view? Then exit state.
    }
}
