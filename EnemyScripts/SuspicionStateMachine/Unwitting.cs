using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unwitting : UpdatableState<SuspicionStateMachine>
{
    float timeToNotice = 1; //0.25f;
    float noticeStopwatch = 0;
    public override void enter(SuspicionStateMachine machine)
    {
        noticeStopwatch = 0;
    }

    protected override void exit(SuspicionStateMachine machine)
    {
        machine.enterState(machine.Noticing);
    }

    public override void update(SuspicionStateMachine machine)
    {
        // Have we noticed? If so, immediately go to next state.
        if (machine.Controller.Look.isPlayerInSight_Calculate())
        {
            // Line effect! //////////////////////////////////////////////////// <<<<<<<<<<<<<<<<<<<<
            noticeIfPlayerLingers(machine);
        }
        else if (machine.Controller.Vibes.status > 0)
        {
            noticeIfPlayerLingers(machine);
        }
        else
        {
            playerEscaped();
        }
    }

    void noticeIfPlayerLingers(SuspicionStateMachine machine)
    {
        if (machine.Controller.Look.isPlayerUnobstructed())
        {
            noticeStopwatch += TimeKeeper.deltaPlayTime();
            if (noticeStopwatch > timeToNotice)
            {
                exit(machine);
            }
        }
        else
        {
            playerEscaped();
        }
    }

    void playerEscaped()
    {
        noticeStopwatch = 0;
    }
}
