namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class TrialWaypointManager : WaypointManager
    {
        //public TrialStateManager Trial;
        public Waypoint[] waypoints;
        public HaltPoint[] haltpoints;
        public int currentWaypoint;
        public int currentHaltpoint;

        public void Reset()
        {
            currentHaltpoint = 0;
            //currentHaltpoint = 0;
            //while (haltpoints[currentHaltpoint].placement == HaltPoint.Standing.FIRST)
            //{
            //    currentHaltpoint++;
            //}
            currentWaypoint = 0;
        }

        public void beginTripToNextHaltpoint()
        {
            currentHaltpoint++;
            currentWaypoint++;
        }

        public Vector3 getNextPosition()
        {
            return waypoints[currentWaypoint].transform.position;
        }

        // Increments waypoint index, returns whether we're at halt
        public bool hasReachedWaypoint()
        {
            bool hasReachedHalt = waypoints[currentWaypoint] == haltpoints[currentHaltpoint];
            currentWaypoint++;
            return hasReachedHalt;
        }

        public HaltPoint getHalt()
        {
            return haltpoints[currentHaltpoint];
        }


        ////public void Awake()
        ////{
        ////    lineRenderer.enabled = false;
        ////}

        //public void assignWaypoints()
        //{
        //    Trial.waypoints = new TrialWaypoint[this.transform.childCount];
        //    float durationSum = 0;
        //    for (int i = 0; i < this.transform.childCount; i++)
        //    {
        //        Trial.waypoints[i] = this.transform.GetChild(i).GetComponent<TrialWaypoint>();
        //        Trial.waypoints[i].setup((i < this.transform.childCount-1) ? this.transform.GetChild(i + 1) : null, durationSum);
        //        durationSum += Trial.waypoints[i].normalLegDurationSec;
        //    }
        //}

        //public void Update()
        //{
        //    for (int i = 0; i < this.transform.childCount; i++)
        //    {
        //        GameObject G = this.transform.GetChild(i).gameObject;
        //        G.GetComponent<TrialWaypoint>().drawGizmos = drawGizmos;
        //    }
        //}
    }
}