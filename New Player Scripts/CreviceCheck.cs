using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreviceCheck : MonoBehaviour
{
    public FadeSardine fader;
    public float closeDistance = 0.25f;
    // What is considered really close for SUPER CREVICE mode
    public float superCloseDist = 0.2f;/*0.011f;*/
    public float fishCamCloseDistSqr = 1;

    // Overlap detection
    public float checkRadiusCameraGround = 0.01f;
    public float checkRadiusCameraFish = 0.01f;
    public Vector3 checkAboveHalfExtents = new Vector3(0.1f, 0.1f, 0.3f);
    public Vector3 checkHalfExtentsFish = new Vector3(0.1f, 0.1f, 0.3f);
    public float tunnelCheckDistance = 3f;
    public LayerMask playerMask;
    public LayerMask creviceMask;
    public Transform checkPointEnd;
    public Transform checkPointStart;
    public Transform checkAbovePoint;

    // Flags, used by this and other scripts
    public static bool camFishOverlap = false;
    public static bool camGroundOverlap = false;
    public static bool fishGroundOverlap = false;
    public static bool inCrevice = false;
    public static bool inSuperCrevice = false;
    public static bool inTunnel = false;
    public static bool onSlopeBelow = false;
    public static bool onSlopeDown = false;
    public static bool upHill = false;
    public static bool aboveOpen = false;

    public static int above, below, left, right;

    int numCreviceChecks = 4;
    int creviceIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (fader.inOracle)
            return;
        //Debug.Log("camFishOverlap " + camFishOverlap);
        //Debug.Log("fishGroundOverlap " + fishGroundOverlap);
        //Debug.Log("inCrevice " + inCrevice);

        //superCreviceCheck();
        overlapChecks();
        setCreviceFlags();

        //if (MainCameraController.currentCamera == VirtualCameraManager.VirtualCam.SUPERCREVICE)   // Super Crevice Disapperence Needed
        //{
        //    fader.disappear();
        //}
        //else if (camFishOverlap)
        //{
        //    fader.fadeOut();
        //}
        //else
        //{
        //    fader.fadeIn();
        //}
    }

    //private void FixedUpdate()
    //{
    //    // Camera is close if the overlap sphere reaches us.

    //    //overlapChecks();

    //    //inCrevice = camFishOverlap && fishGroundOverlap && Physics.OverlapSphere(Camera.main.transform.position, checkRadiusCamera, creviceMask).Length >= 1; // camera overlapping fish, fish overlapping ground, and camera overlapping ground
    //}

    void overlapChecks()
    {
        if (creviceIndex == 0)
            camFishOverlap = Physics.OverlapSphere(Camera.main.transform.position, checkRadiusCameraFish, playerMask).Length >= 1;  // camera overlapping fish
        else if (creviceIndex == 1)
            fishGroundOverlap = Physics.OverlapBox(checkPointStart.position, checkHalfExtentsFish, this.transform.rotation, creviceMask).Length >= 1;
        else if (creviceIndex == 2)
            aboveOpen = Physics.OverlapBox(checkAbovePoint.position, checkAboveHalfExtents, this.transform.rotation, creviceMask).Length < 1;
        else if (creviceIndex == 3)
            camGroundOverlap = Physics.OverlapSphere(Camera.main.transform.position, checkRadiusCameraGround, creviceMask).Length >= 1;

        // Tunnel (this is inexpensive, so I can do it every frame)
        above = (AreaCheck.aboveHit < tunnelCheckDistance) ? 1 : 0;
        below = (AreaCheck.belowHit_Local < tunnelCheckDistance) ? 1 : 0;
        left  = (AreaCheck.leftHit < tunnelCheckDistance) ? 1 : 0;
        right = (AreaCheck.rightHit < tunnelCheckDistance) ? 1 : 0;
        inTunnel = above + below == 2 || above + below + left + right > 2; // (above + below + left + right >= 2);
        //inTunnel = (above + below + left + right >= 2) || (above == 1);


        // Increment
        creviceIndex++;
        if (creviceIndex >= numCreviceChecks)
            creviceIndex = 0;
    }

    //void superCreviceCheck()
    //{
    //    int counter = 0;
    //    if (AreaCheck.aboveHit < superCloseDist)
    //        counter++;
    //    if (AreaCheck.belowHit < superCloseDist)
    //        counter++;
    //    if (AreaCheck.leftHit < superCloseDist)
    //        counter++;
    //    if (AreaCheck.rightHit < superCloseDist)
    //        counter++;
    //    inSuperCrevice = inCrevice && counter > 2;
    //}

    void setCreviceFlags()
    {
        // In crevice if near object on the fish's left, right, and below, and if the camera is close enough to the fish
        //inCrevice = AreaCheck.leftHit < closeDistance && AreaCheck.rightHit < closeDistance && AreaCheck.belowHit < closeDistance && (this.transform.position - Camera.main.transform.position).sqrMagnitude < fishCamCloseDistSqr;
        onSlopeBelow = aboveOpen && AreaCheck.isSlopeBelow();
        onSlopeDown = aboveOpen && AreaCheck.isSlopeDown();
        //Debug.Log("----------");
        //Debug.Log("adoveOpen: " + aboveOpen);
        //Debug.Log("AreaCheck.isSlope: " + AreaCheck.isSlope);
        //Debug.Log("MainCameraController.isTiltingDown: " + MainCameraController.isTiltingDown);
        //Debug.Log("DownSlope: " + downSlope);//
        inCrevice = !onSlopeDown && fishGroundOverlap && camFishOverlap;//(this.transform.position - Camera.main.transform.position).sqrMagnitude < fishCamCloseDistSqr;
        inSuperCrevice = !onSlopeDown && inCrevice && (camFishOverlap && /*||*/ camGroundOverlap);
        // In super crevice if near object above fish too
        //inSuperCrevice = inCrevice && AreaCheck.aboveHit < closeDistance;
    }

    public void OnDrawGizmos()
    {
        // These two lines by Talzor at https://forum.unity.com/threads/gizmo-rotation.4817/
        //Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, this.transform.rotation, transform.lossyScale);
        //Gizmos.matrix = rotationMatrix;
        if (Physics.OverlapBox(checkPointStart.position, checkHalfExtentsFish, this.transform.rotation, creviceMask).Length >= 1)
        {
            Gizmos.DrawCube(checkPointStart.position, checkHalfExtentsFish*2);
        }
        else
        {
            Gizmos.DrawWireCube(checkPointStart.position, checkHalfExtentsFish*2);
        }
        
        if (Physics.OverlapBox(checkAbovePoint.position, checkAboveHalfExtents, this.transform.rotation, creviceMask).Length < 1)
        {
            Gizmos.DrawCube(checkAbovePoint.position, checkAboveHalfExtents * 2);
        }
        else
        {
            Gizmos.DrawWireCube(checkAbovePoint.position, checkAboveHalfExtents * 2);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.up * tunnelCheckDistance));
        Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.up * -tunnelCheckDistance));
        Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.right * tunnelCheckDistance));
        Gizmos.DrawLine(this.transform.position, this.transform.position + (this.transform.right * -tunnelCheckDistance));


        Gizmos.DrawWireSphere(Camera.main.transform.position, checkRadiusCameraFish);
    }
}
