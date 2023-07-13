namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [System.Serializable]
    public class StateManager : MonoBehaviour
    {
        public TrialWaypointManager waypointManager;
        public Movement Move;
        public Timer timer;
        public float speed = 40;

        public float overlapSphereRadius = 3;

        public SpiritMovement SpiritMaster;



        public Stopwatch stopwatch;
        public Waypoint[] waypoints;
        public TrialBubbleAppearance bubbleLook;
        public TrialSounds sounds;
        public ParticleSystem winPart;
        public float playerTooFarSqrt = 160;
        public float playerPathTooFarSqrt = 160;
        public float outOfRangeMaxSec = 3;
        public Vector3 startPos;
        public TrialSaveLoad saverLoader;

        [Space]

        public float currentBestScore = -1;

        BaseState currentState;
        public IdleState idleState = new IdleState();
        public StartState startState = new StartState();
        public TravellingState travelState = new TravellingState();
        public HaltState haltState = new HaltState();
        public LoseState loseState = new LoseState();
        public ReturnState returnState = new ReturnState();
        public WinState winState = new WinState();

        public TrialMovement movement;
        public DistanceFromPath pathDistance;
        public float timeOutBounds = -1;
        public float maxSpeedRelativeToPlayer;

        public TrialBubbleDrift bubbleDrift;

        private void Awake()
        {
            //movement = new TrialMovement(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            currentState = idleState;//waitState;
            currentState.enterState(this);
            //Debug.Log("beginning at state " + currentState);//

            //string s = "";
            //s = saverLoader.id + " ";
            //foreach (TrialWaypoint W in waypoints)
            //{
            //    s += W.transform.position + " ";
            //}
            //Debug.Log(s);
        }

        // Update is called once per frame
        void Update()
        {
            currentState.updateState(this);
            // TO DO: Better layer masking
            // Trial movement visual - restore
            if (Physics.OverlapSphere(waypointManager.getHalt().transform.position, overlapSphereRadius, LayerMask.GetMask("Player")).Length != 0) // If the player has collided with the next point
                currentState.playerInRadius(this);
        }

        public void switchState(BaseState state)
        {
            Debug.Log("Switching to state " + state);//
            currentState = state;
            state.enterState(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(waypointManager.getHalt().transform.position, overlapSphereRadius);
        }

        //public void OnTriggerEnter(Collider other)
        //{
        //    currentState.onTriggerEnter(this, other);
        //}
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StateManager))]
    public class TrialStateManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            StateManager script = (StateManager)target;

            if (GUILayout.Button("Go!"))
            {
                script.switchState(script.startState);
            }
        }
    }
#endif
}