using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlivingAnimation : MonoBehaviour
{
    public Animator animationController;
    public NPCMovement movement;
    public float baseMovementSpeed = 1.25f;

    public void Update()
    {
        setSpeed(movement.adjustedTargetSpeed);
    }

    public void attack()
    {
        animationController.SetTrigger("attack");
        animationController.SetTrigger("attack");
    }

    public void setSpeed(float speed)
    {
        if (speed < baseMovementSpeed)
            speed = baseMovementSpeed;

        animationController.SetFloat("ChaseMultiplier", speed);
    }

    //public void swim()
    //{
    //    //animationController.SetTrigger("swim");
    //}

    public void spotted()
    {
        animationController.SetTrigger("spotted");
    }

    public void idle()
    {
        animationController.SetTrigger("idle");
    }
}
