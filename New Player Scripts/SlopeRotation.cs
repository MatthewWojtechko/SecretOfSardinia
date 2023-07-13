using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeRotation : MonoBehaviour
{
    public float raycastDist = 1;
    public LayerMask obstacles;

    RaycastHit hit;
    bool hasHit;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        getMaxRotation();
    }

    private void FixedUpdate()
    {
        hasHit = Physics.Raycast(this.transform.position, this.transform.up * -1, out hit, raycastDist, obstacles);
    }

    public float getMaxRotation()
    {
        //Debug.Log(hit.normal);//
        //if (hasHit)
        //    Debug.Log(Mathf.Rad2Deg * Mathf.Acos(hit.normal.y));
        if (hasHit)
        {
            float degrees = Mathf.Rad2Deg * Mathf.Acos(hit.normal.y);
            //Debug.Log(degrees);
            if (degrees > 90)
                return degrees;
        }

        return 90;
    }
    // https://forum.unity.com/threads/rotation-normal-xyz-1-to-1-to-degrees-images.497738/

}
