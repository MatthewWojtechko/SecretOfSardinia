using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVibes : MonoBehaviour
{
    public Vibe[] vibes;
    //public Vector3 center = Vector3.zero;
    public Transform eyes;

    //public Color debugColor = Color.green;
    //public float debugRadius = 0.1f;
    //public Vector3 debugWorldPosition;

    public VibeStatus status;
    public Vector3 playerLastPosition;

    public enum VibeStatus { NONE, FEEL, KNOW};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vibes == null || vibes.Length == 0)
            return;
    }

    public void updateVibeStatus(Vector3 playerPosition, float playerDistanceSqr)
    {
        if (vibes == null)
        {
            Debug.LogWarning("This enemy's vibe list is not set. It cannot sense if the player is closeby and outside its field of view.");
            return;
        }

        if (vibes.Length > 0 && isPlayerWithinZone(vibes[0], playerPosition, playerDistanceSqr))
        {
            status = VibeStatus.KNOW;
            playerLastPosition = playerPosition;
        }
        else if (vibes.Length > 1 && isPlayerWithinZone(vibes[1], playerPosition, playerDistanceSqr))
        {
            status = VibeStatus.FEEL;
            playerLastPosition = playerPosition;
        }
        else
            status = VibeStatus.NONE;
    }

    bool isPlayerWithinZone(Vibe vibe, Vector3 playerPosition, float playerDistanceSqr)
    {
        //float playerDistanceSqr = Vector3.SqrMagnitude((eyes.transform.position) - playerPosition);
        if (playerDistanceSqr < vibe.radius * vibe.radius)     // Player is within the sphere
        {
            ////Vector3 playerLocalPosition = this.transform.InverseTransformPoint(playerPosition); //this.transform.rotation * playerPosition;
            //Debug.Log(playerLocalPosition);
            ////if (playerLocalPosition.y < vibe.maxHeight && playerLocalPosition.y > vibe.minHeight)
            ////{
                return true;
            ////}
        }
        return false;
    }

    void drawVibe(Vibe vibe)
    {
        Gizmos.color = vibe.gizmoColor;
        //Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        //Gizmos.DrawSphere(eyes.transform.localPosition, vibe.radius);
        Gizmos.DrawSphere(eyes.transform.position, vibe.radius);
        
        //drawRoof(vibe.maxHeight, vibe.radius, eyes.transform.localPosition);
        //drawRoof(vibe.minHeight, vibe.radius, eyes.transform.localPosition);

        //void drawRoof(float altitude, float radius, Vector3 center)
        //{
        //    int sign = 1;
        //    if (altitude < 0)
        //        sign = -1;
        //    float boxHeight = radius - Mathf.Abs(altitude);
        //    Gizmos.DrawCube(new Vector3(0, altitude + (sign * boxHeight/2), 0) + center, new Vector3(vibe.radius * 2, boxHeight, vibe.radius * 2) + center);
        //}
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = debugColor;
        //Gizmos.DrawSphere(debugWorldPosition, debugRadius);
        ////Debug.Log("Within: " + isPlayerWithinZone(vibes[0], debugWorldPosition));
        //updateVibeStatus(debugWorldPosition);
        //Debug.Log(status);


        //////
        if (vibes == null || vibes.Length == 0)
            return;

        foreach (Vibe v in vibes)
        {
            if (v.drawGizmo)
                drawVibe(v);
        }
    }

    public Vector3 getSensedPos()
    {
        if (status != VibeStatus.NONE)
            return SardineSwim.playerTransform.position;
        else
            return playerLastPosition;
    }

    [System.Serializable]
    public class Vibe
    {
        public float radius;
        public float maxHeight;
        public float minHeight;
        public bool drawGizmo;
        public Color gizmoColor;
    }
}