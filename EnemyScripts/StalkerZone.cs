using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalkerZone : MonoBehaviour
{
    public Pathing.StalkPathMaster PathMaster;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayersAndTags.playerLayer)
            playerEnter();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayersAndTags.playerLayer)
            PlayerExit();
    }

    void playerEnter()
    {
        PathMaster.positionPoint = SardineSwim.playerTransform;
        PathMaster.currentState = Pathing.PathState.SETTING;
        PathMaster.begin(SardineSwim.playerTransform.position);
    }

    void PlayerExit()
    {
        PathMaster.currentState = Pathing.PathState.NONE;
    }
}
