using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerReactor : MonoBehaviour
{
    public LayerMask collideWithMask;
    public int numInRange = 0;
    public Vector3 lastPosition;

    public void OnTriggerEnter(Collider other)
    {
        if (collideWithMask != (collideWithMask | (1 << other.gameObject.layer)))
            return;
        numInRange++;
    }
    public void OnTriggerExit(Collider other)
    {
        if (collideWithMask != (collideWithMask | (1 << other.gameObject.layer)))
            return;
        numInRange--;
        if (numInRange == 0)
            lastPosition = SardineSwim.playerTransform.position;
    }

    public bool isColliding()
    {
        return numInRange > 0;
    }

}
