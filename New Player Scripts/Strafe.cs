using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Strafe : MonoBehaviour
{
    public DiveBoost diveScript;
    public float speedFactor = 3;
    public float fastSpeedFactor = 5;
    public float autoRotSlerp = 5;
    public float fastAutoRotSlerp = 0.1f;
    public Rigidbody rigid;
    public InputParticles.Effect strafeEffect;

    private float slerp;

    public Controls Controls;
    public Swimput currentInput;

    public static event Action onStrafeStart;
    public static event Action onEndStrafe;

    public bool isStrafing;
    public bool strafeDebugOverride = false;

    public void Awake()
    {
        endStrafe();
    }

    //public void Update()
    //{
    //    //Debug.Log("left and right: " + this.transform.rotation.eulerAngles.y);
    //    Debug.Log("up and down: " + this.transform.rotation.eulerAngles.x);
    //}

    //private void OnEnable()
    //{
    //    Controls.Swim.Strafe.started += _ => lockYaw();
    //    Controls.Swim.Strafe.canceled += _ => unlockYaw();
    //}

    //private void OnDisable()
    //{
    //    PlayerCharacterInput.onStrafePressed -= lockYaw;
    //    PlayerCharacterInput.onStrafeReleased -= unlockYaw;
    //}

    public void move()
    {
        //// Won't work with free rotate!
        //rigid.AddForce(PlayerCharacterInput.currentInput.horizontal * (Playe
        //rCharacterInput.currentInput.horiztonalFast ? fastSpeedFactor : speedFactor) * Camera.main.transform.right);
        //rigid.AddForce(PlayerCharacterInput.currentInput.vertical * (PlayerCharacterInput.currentInput.verticalFast ? fastSpeedFactor : speedFactor) * (1 + diveScript.diveBoostFactor) * -1 * Vector3.up);

        //rigid.MoveRotation(Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0), autoRotSlerp));

        if (currentInput.rotationFast)
            slerp = fastAutoRotSlerp;
        else
            slerp = autoRotSlerp;

        rigid.MoveRotation(Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(-90 * currentInput.rotation.y, (90 * currentInput.rotation.x) + LookPointParent.yaw, 0), slerp));
    }

    // Begins/ends strafe. Locks/unlocks yaw, plays effect.
    //public void onStrafe(InputAction.CallbackContext context)
    //{
    //    //Debug.Log("on strafe");//
    //    //if (context.performed)
    //    //{
    //    //    Debug.Log("strafe started");//
    //    //    InputParticles.play(ref strafeEffect);
    //    //    LookPointParent.yaw = SardineSwim.playerTransform.rotation.eulerAngles.y;
    //    //    yawFree = false;
    //    //    onStrafeStart?.Invoke();
    //    //}
    //    //else if (context.canceled)
    //    //{
    //    //    Debug.Log("strafe canceled");//
    //    //    yawFree = true;
    //    //    LookPointParent.needsLerped = true;
    //    //    onEndStrafe?.Invoke();
    //    //}

    //    if (context.started)
    //    {
    //        if (isStrafing)
    //            endStrafe();
    //        else
    //            startStrafe();
    //    }
    //}

    public void startStrafe()
    {
        if (strafeDebugOverride) //
        {
            isStrafing = false;
            return;
        }
        LookPointParent.rotateWithPlayer();

        //Debug.Log("strafe started");//
        isStrafing = true;
        onStrafeStart?.Invoke();

        LookPointParent.isStrafing = true;

    }

    public void endStrafe()
    {
        if (strafeDebugOverride) //
        {
            isStrafing = false;
            return;
        }

        InputParticles.play(ref strafeEffect);

        //Debug.Log("strafe canceled");//
        LookPointParent.needsLerped = true;
        isStrafing = false;

        SardineSwim.yAxisRotationAmount = this.transform.rotation.eulerAngles.y % 360;

        //LookPointParent.rotateWithPlayer();
        LookPointParent.isStrafing = false;

        onEndStrafe?.Invoke();
    }

    //public void unlockYaw()
    //{
    //    yawFree = true;
    //    LookPointParent.needsLerped = true;
    //}


}
