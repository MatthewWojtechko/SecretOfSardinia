using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionStateMachine
{
    public EnemyController Controller;

    [HideInInspector] public UpdatableState<SuspicionStateMachine> Unwitting;
    [HideInInspector] public UpdatableState<SuspicionStateMachine> Noticing;
    [HideInInspector] public UpdatableState<SuspicionStateMachine> Curious;
    [HideInInspector] public UpdatableState<SuspicionStateMachine> Seeing;
    [HideInInspector] public UpdatableState<SuspicionStateMachine> Eluded;

    /*[HideInInspector] */public UpdatableState<SuspicionStateMachine> currentState;

    public SuspicionStateMachine(EnemyController con)
    {
        Controller = con;

        Unwitting = new Unwitting();
        Noticing = new Noticing();
        Curious = new Curious();
        Seeing = new Seeing();
        Eluded = new Eluded();

        enterState(Unwitting);
    }

    public void enterState(UpdatableState<SuspicionStateMachine> state)
    {
        state.enter(this);
        currentState = state;
        //Debug.Log("current thought state: " + currentState.GetType().ToString());//
    }

    public void UpdateCode()
    {
        currentState.update(this);
    }


}
