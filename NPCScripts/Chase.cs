using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public NPCMovement movement;
    public EnemyLook sight;
    public float maxSwimSpeed = 10;
    public float swimForce = 200;
    public float turnLerp = 0.015f;
    public bool inSight;

    public Vector3 lastSeen;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    movement.setPointMovement(maxSwimSpeed, swimForce, turnLerp);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (inSight)
    //    {
    //        movement.singlePoint = Constants.instance.playerPosition;

    //        if (!sight.visibleInChase)
    //        {
    //            lastSeen = Constants.instance.playerPosition;
    //            inSight = false;
    //        }
    //    }
    //    else
    //    {
    //        movement.singlePoint = lastSeen;
    //        if (sight.visibleInChase)
    //        {
    //            inSight = true;
    //        }
    //    }
        
    //}
}
