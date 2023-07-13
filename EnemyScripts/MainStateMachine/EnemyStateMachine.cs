using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyStateMachine
{
    public Breed species;
    public EnemyController Controller;
    public VariableSpawner spawnerProtection;
    int returnIndex = 0;

    private PhysicalState<EnemyStateMachine>[] states;
    [HideInInspector] public int currentState;
    [HideInInspector] public int PATROL, STARTLED, 
                                 INVESTIGATE, ALARM, CHASE, ATTACK, CHASEEND, RETURN,
                                 HIDE, JUMP,
                                 FOLLOW, INITIATE, ESCAPE,
                                 LOST;

    public bool isRunning = false;

    public PhysicalState<EnemyStateMachine> getCurrentState()
    {
        return states[currentState];
    }

    public EnemyStateMachine(EnemyController C)
    {
        Controller = C;
        //switch (species)
        //{
        //    case Breed.PATROLLER:
                states = new PhysicalState<EnemyStateMachine>[7];

                states[0] = new Patrolling();
                PATROL = 0;

                //states[1] = new Startled();
                //STARTLED = 1;

                states[1] = new Investigating();
                INVESTIGATE = 1;

                states[2] = new Alarming();
                ALARM = 2;

                states[3] = new Chasing();
                CHASE = 3;

                states[4] = new Attacking();
                ATTACK = 4;

                states[5] = new ChaseEnd();
                CHASEEND = 5;

                states[6] = new Returning();
                RETURN = 6;
        //        break;

        //    case Breed.HIDER:
        //        states = new PhysicalState<EnemyStateMachine>[8];

        //        states[0] = new Hiding();
        //        HIDE = 0;

        //        states[1] = new Jumping();
        //        JUMP = 1;

        //        states[2] = new Investigating();
        //        INVESTIGATE = 2;

        //        states[3] = new Alarming();
        //        ALARM = 3;

        //        states[4] = new Chasing();
        //        CHASE = 4;

        //        states[5] = new Attacking();
        //        ATTACK = 5;

        //        states[6] = new ChaseEnd();
        //        CHASEEND = 6;

        //        states[7] = new Returning();
        //        RETURN = 7;
        //        break;

        //    case Breed.STALKER:
        //        states = new PhysicalState<EnemyStateMachine>[5];

        //        states[0] = new Stalker_Following();
        //        FOLLOW = 0;

        //        states[1] = new Stalker_Initiate();
        //        INITIATE = 1;

        //        states[2] = new Stalker_Chasing();
        //        CHASE = 2;

        //        states[3] = new Stalker_Eluded();
        //        CHASEEND = 3;

        //        states[4] = new Stalker_Evade();
        //        ESCAPE = 4;
        //        break;
        //}

        enterState(0);
    }

    public void enterState(int nextState)
    {
        // Enter the next state. Then, mark that it is now the current state.
        states[nextState].enter(this);
        currentState = nextState;

        // If we just switched to state 1, then that means this Unliving is starting to interact with the player. That means it needs protection from the World Streamer.
        if (spawnerProtection != null)
        {
            if (currentState == 1)
            {
                spawnerProtection.protect();
                returnIndex = Controller.PatrollerController.incrementIndex(Controller.PatrollerController.currentWaypoint);  // We will know this Unliving no longer needs protection when it gets back to the next waypoint.
            }
        }
        //Debug.Log("current action state: " + currentState);//
    }
    public void UpdateCode()
    {
        states[currentState].update(this);

        // If we are back to the base state and have arrived at the waypoint that we left at, then we truly are back to where we began. That means we no longer need protection from the World Streamer, as we are in our usual place.
        if (spawnerProtection != null)
        {
            if (currentState == 0 && Controller.PatrollerController.currentWaypoint == returnIndex)
                spawnerProtection.doNotProtect();
        }
    }
    public void FixedUpdateCode()
    {
        states[currentState].fixedUpdate(this);
    }
}

[SerializeField]
public enum Breed { PATROLLER, HIDER, STALKER };

//[CustomEditor(typeof(EnemyStateMachine))]
//public class EnemyStateMachineEditor : Editor
//{
//    public float delayTime;
//    public int state;
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        EnemyStateMachine script = (EnemyStateMachine)target;

//        state = EditorGUILayout.IntField(state);
//        delayTime = EditorGUILayout.FloatField(delayTime);

//        if (GUILayout.Button("Enter State"))
//        {
//            script.StartCoroutine(delayEnterState(state, delayTime, script));
//        }


//    }

//    public IEnumerator delayEnterState(int state, float delay, EnemyStateMachine script)
//    {
//        yield return new WaitForSeconds(delay);
//        script.enterState(state);
//    }
//}