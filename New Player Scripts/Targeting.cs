using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [Header("Object Detection")]
    public LayerMask objOfInterestMask;
    public LayerMask obstacleMask;
    public float capsuleOffsetNear;
    public float capsuleOffsetFar;
    public float capsuleRadius;

    [Header("Movement Specifications")]
    public float directionalMag = 10;
    public float upDownMag = 10;
    public float leftRightMag = 100;
    public float leftRightAngleFactor = 0.1f;

    public float rotStrength;
    public float maxRotMag = 10;
    public float centerMagFactor = 5;
    public float maxCenterMag = 3;
    public Rigidbody rigid;

    private Collider closestCollider;
    private float tempDifference;

    public Swimput currentInput;

    private void FixedUpdate()
    {
        findObjects();
        //rotateToward();//
    }

    public void activate()
    {
        rotateToward();
        directionalForce();
        applyCenteringForce();
    }

    private void findObjects()
    {
        setClosest(Physics.OverlapCapsule(this.transform.position + (this.transform.forward * capsuleOffsetNear), this.transform.position + (this.transform.forward * capsuleOffsetFar), capsuleRadius, objOfInterestMask));
    }

    // Returns the closest game object in the list. If the list is empty, returns false.
    private void setClosest(Collider[] nearbyColliders)
    {
        closestCollider = null;
        float closestSqrDist = Mathf.Infinity;

        // Exit if none found
        if (nearbyColliders.Length <= 0)
        {
            return;
        }

        // Find the closest collider
        check(nearbyColliders[0]);
        for (int i = 1; i < nearbyColliders.Length; i++)
        {
            check(nearbyColliders[i]);
        }

        void check(Collider C)
        {
            if (!Physics.Linecast(this.transform.position, C.transform.position, obstacleMask))
            {
                tempDifference = (this.transform.position - C.transform.position).sqrMagnitude;
                if (tempDifference < closestSqrDist)
                {
                    closestCollider = C;
                    closestSqrDist = tempDifference;
                }
            }
        }
    }

    // Rotate toward the target object.
    public void rotateToward()
    {
        // Code from: http://wiki.unity3d.com/index.php?title=TorqueLookRotation&oldid=13941

        Vector3 targetDelta = (closestCollider.transform.position - rigid.position).normalized;

        // Get the angle between our forward and the direction from here to target.
        float angleDiff = Vector3.Angle(this.transform.forward, targetDelta);

        // Calculate its cross product, which is the axis of rotation from one vector to another.
        Vector3 cross = Vector3.Cross(this.transform.forward, targetDelta).normalized;

        // Apply torque along that axis, with the magnitude determined from both the above field and how far we have to rotate.
        rigid.AddTorque(cross * angleDiff * rotStrength);
        //this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, 0);
    }

    void applyCenteringForce()
    {
        Vector3 relPos = this.transform.InverseTransformPoint(closestCollider.transform.position);
        rigid.AddForce(relPos.y * centerMagFactor * this.transform.up);
        float mag = Mathf.Clamp(maxCenterMag - (relPos.y * centerMagFactor), -maxCenterMag, maxCenterMag);
        rigid.AddForce(mag * this.transform.up);
        mag = Mathf.Clamp(maxCenterMag - (relPos.x * centerMagFactor), -maxCenterMag, maxCenterMag);
        rigid.AddForce(mag * this.transform.right);
    }

    // Move the player up/down/left/right, depending on the player's directional input.
    public void directionalForce()
    {
        rigid.AddForce(((currentInput.rotation.x * Camera.main.transform.right) + (-currentInput.rotation.y * Vector3.up)) * directionalMag);
    }

    public bool inRange()
    {
        return closestCollider != null;
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(this.transform.position + (this.transform.forward * capsuleOffsetNear), capsuleRadius);
    //    Gizmos.DrawSphere(this.transform.position + (this.transform.forward * capsuleOffsetFar), capsuleRadius);
    //    if (closestCollider != null)
    //        Gizmos.DrawCube(closestCollider.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    //}
}
