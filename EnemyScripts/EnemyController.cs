using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyController : MonoBehaviour
{
    public UnlivingAnimation Animation;
    public NPCMovement Movement;
    public PatrolMovement PatrollerController;
    public EnemyLook Look;
    public EnemyVibes Vibes;
    public SightKnowledge Knowledge;
    public Pathing.PathMaster Pather;
    public SightLineManager LineManager;
    public Rigidbody rigidbody;
    public Collider[] colliders;
    public bool isActivated = false;

    public EnemyStateMachine Behavior;
    public SuspicionStateMachine Suspicion;
    public SightEffects effects;

    [Range(1, 50)]public int sizeScale = 1;

    public float eludedDuration = 1;
    public float damage = 0.5f;

    public static float maxVagueZoneRadius = 9;

    public void Awake()
    {
        Behavior = new EnemyStateMachine(this);
        Suspicion = new SuspicionStateMachine(this);
        effects = new SightEffects(this);

        rigidbody.Sleep();
        WranglerHub.enemies.register(this);
    }

    public void OnDestroy()
    {
        WranglerHub.enemies.deregister(this);
    }

    private void OnValidate()
    {
        this.transform.localScale = Vector3.one * sizeScale;
        Look.multiplier = sizeScale;
        //CapsuleCollider collider = this.transform.GetChild(1).GetComponent<CapsuleCollider>();
        //if (collider.radius >= maxVagueZoneRadius)
        //    collider.radius = maxVagueZoneRadius;
    }

    //public float spherecastRadius = 0.1f;
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(EnemyController))]
//public class EnemyControllerEditor : Editor
//{

//}
//#endif

