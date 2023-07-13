/*
 * Calculates how much extra force (boost) the player gets from swimming quickly downwards (diving).
 * The amount of force increases as you dive downwards. It decreases the farther the player is from the downward orientation.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DiveBoost : MonoBehaviour
{
    public float terminalBoost;
    public float acceleration;
    public float fastAcceleration;
    public bool strafeDive;

    public float nosePointOffset; // In neutral rotation, how far is nose point from the center?
    public Transform nosePoint;

    public float diveBoostFactor = 0;
    public float diveBoostDecelLerp = 0.2f;

    private float tiltAmount; // -1 is up, 0 is neutral, 1 is down
    public float minimumTilt = 0.5f;

    public Swimput currentInput;
    public float releasedTime;
    public bool released = false;
    public float preserveThruSkimWindow = 0.4f;

    //public void Update()
    //{
    //    //Debug.Log(Mathf.Clamp(((this.transform.position.y - nosePoint.position.y) / nosePointOffset), -1, 1));
    //}

    public void loseIt()
    {
        releasedTime = Time.time;
        released = true;
    }

    public void onForwardRelease(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            releasedTime = Time.time;
            released = true;
        }
    }

    public void onSkimStart()
    {
        released = false;
    }    

    public void calculate()
    {
        // If strafe-boosting, then boost, then exit the function.
        //if (PlayerCharacterInput.currentInput.vertical > 0.9f && PlayerCharacterInput.currentInput.target)
        //{
        //    boost(1);
        //    return;
        //}

        // Calculates how far the fish's nose is below its center.
        tiltAmount = Mathf.Clamp(((this.transform.position.y - nosePoint.position.y) / nosePointOffset), -1, 1);

        if (released && Time.time - releasedTime >= preserveThruSkimWindow)
        {
            diveBoostFactor = 0;
            released = false;
        }
        else if (tiltAmount < minimumTilt)  // If not tilted downward enough, deplete the boost..
        {
            diveBoostFactor = Mathf.Lerp(diveBoostFactor, 0, diveBoostDecelLerp * Time.deltaTime);
        }
        else                          // Otherwise, accelerate it.
        {
            boost(tiltAmount);
        }
    }

    void boost(float tiltAmnt)
    {
        float activeAccel = acceleration;
        if (currentInput.flickDown())     // If the player flicks downward, speed up faster.
        {
            ////Debug.Log("FAST");
            activeAccel = fastAcceleration;
        }
        diveBoostFactor += Mathf.Clamp(tiltAmnt * activeAccel, 0, terminalBoost) * Time.deltaTime;
        if (diveBoostFactor > terminalBoost)
            diveBoostFactor = terminalBoost;
    }
}
