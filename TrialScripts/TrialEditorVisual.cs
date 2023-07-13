namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EditorVisual : CustomHierachyVisual
    {
        public StateManager trial;

        public override bool displayVisual()
        {
            if (trial.waypoints == null)
                return true;
            if (trial.waypoints.Length == 0)
                return true;
            foreach (Waypoint wp in trial.waypoints)
            {
                if (wp == null)
                    return true;
            }
            return false;
        }
    }
}