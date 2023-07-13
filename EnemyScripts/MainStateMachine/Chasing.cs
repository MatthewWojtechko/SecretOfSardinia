using Pathing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : EnemyState
{
    public static Action enemyBeginDeathStare;
    public static Action enemyEndDeathStare;
    bool attack = false;
    bool wasSeeing;
    public override void enter(EnemyStateMachine machine)
    {
        // animation needed?
        attack = false;
        machine.Controller.Pather.embarking();
        //enemyBeginDeathStare?.Invoke();
    }

    protected override void exit(EnemyStateMachine machine)
    {
        if (attack)
            machine.enterState(machine.ATTACK);
        else
        {
            machine.enterState(machine.CHASEEND);
        }
    }

    public override void fixedUpdate(EnemyStateMachine machine)
    {
        // move
        machine.Controller.Movement.moveTo(machine.Controller.Knowledge.getPlayerLastPos(), machine.Controller.Movement.CHASING_SPEED);
    }

    public override void update(EnemyStateMachine machine)
    {
        bool isSeeing = machine.Controller.Suspicion.currentState is Seeing;
        if (!wasSeeing && isSeeing)
            enemyBeginDeathStare?.Invoke();
        else if (wasSeeing && !isSeeing)
            enemyEndDeathStare?.Invoke();
        wasSeeing = machine.Controller.Suspicion.currentState is Seeing;

        if (machine.Controller.Suspicion.currentState is Eluded)
        {
            exit(machine);
        }
        // TODO (?) switch to attack state when nearby...


        // calculate
    }
}
