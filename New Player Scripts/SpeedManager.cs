using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    //Debug
    public string debugBoostButton = "Y";
    public string debugSwimButton = "X";
    public Rigidbody body;

    public float impulseAmount;

    private bool boostFlag = false;
    private bool swimFlag = false;

    public float forceAddition;
    public float forceDecelPerFixed = 1;

    public float regularSwimForce = 10;
    public float slowSwimForce = 3;

    public float swimBoostForce = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(debugSwimButton))
            swimFlag = true;

        if (Input.GetButtonDown(debugBoostButton))
            boostFlag = true;
    }

    void FixedUpdate()
    {
        // Boost
        if (boostFlag)
        {
            //Debug.Log("BOOST!");//
            // impulse
            body.AddForce(this.transform.forward * impulseAmount, ForceMode.Impulse);

            // Add to boost force addition
            swimBoostForce += forceAddition;

            boostFlag = false;
        }

        // Decelerate boost force addition
        if (swimBoostForce > 0)
        {
            swimBoostForce -= forceDecelPerFixed;
        }
        else
        {
            swimBoostForce = 0;
        }

        // Swim
        if (swimFlag)
        {
            body.AddForce(this.transform.forward * (regularSwimForce + swimBoostForce), ForceMode.Force);
            swimFlag = false;
        }
    }

}
