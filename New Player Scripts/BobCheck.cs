using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobCheck : MonoBehaviour
{
    public float inputThreshold = 0.9f;
    public float stateMaxSeconds = 1f;
    public float maxNeitherDuration = 0.5f;

    private float diveTime = -10;
    private float riseTime = -10;
    private float neitherDuration = 0;

    enum BobState { NONE, DIVE, RISE }
    private int statesChained = 0;
    BobState currentState = BobState.NONE;

    public Swimput currentInput;

    public static bool isBobbing;

    // Update is called once per frame
    void Update()
    {
        //if (isDiving())
        //    Debug.Log("DIVING");
        //if (isRising())
        //    Debug.Log("RISING");

        isBobbing = statesChained > 2;
        //Debug.Log(isBobbing);
        //Debug.Log(statesChained);


        // Get at least three chains in a row to qualify as bobbing.
        // If you've been doing neither and you start doing one, that counts as one chain.
        // If you've been doing one, then you start doing the other soon after, that counts as a second one. 
        // But, if you've been doing one, then you do another NOT soon after, that brings the chain back to 1.

        if (isRising())             // Now RISING
        {
            neitherDuration = 0;

            if (Time.time - riseTime > stateMaxSeconds)
                statesChained = 1;

            if (currentState == BobState.DIVE) // has been diving
            {
                riseTime = Time.time;
                statesChained++;
            }
            else if (currentState == BobState.NONE) // has been doing neither
            {
                riseTime = Time.time;
                statesChained = 1;
            }

            currentState = BobState.RISE;
        }
        else if (isDiving())      // Now DIVING
        {
            neitherDuration = 0;

            if (Time.time - diveTime > stateMaxSeconds)
                statesChained = 1;

            if (currentState == BobState.RISE) // has been rising
            {
                diveTime = Time.time;
                statesChained++;
            }
            else if (currentState == BobState.NONE) // has been doing neither
            {
                diveTime = Time.time;
                statesChained = 1;
            }

            currentState = BobState.DIVE;
        }
        else  // Now doing neither
        {
            // Can only do neither for so long, before chain is lost
            neitherDuration += Time.deltaTime;
            if (neitherDuration > maxNeitherDuration)
            {
                statesChained = 0;
                currentState = BobState.NONE;
            }
        }
    }

    bool isDiving()
    {
        return currentInput.flickDown() && currentInput.forward;
    }

    bool isRising()
    {
        return currentInput.flickUp() && currentInput.forward;
    }
}
