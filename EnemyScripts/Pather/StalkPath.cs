namespace Pathing
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StalkPath : Path
    {
        int stalkIndex = 0;

        public override void reset(Vector3 position)
        {
            base.reset(position);
            stalkIndex = 0;
        }

        protected override void incrementSearchIndex()
        {
            searchIndex++;
            if (searchIndex >= nodeCount)
                searchIndex = stalkIndex;
        }

        /*
         * Increments the target. Returns whether the stalker has reached the goal or not.
         */
        public bool stalkerReachedNode()
        {
            stalkIndex++;
            return stalkIndex == nodeCount - 1;
        }

        /*
         * Returns the next position the stalker should move to.
         */
        public Vector3 getNextTarget()
        {
            if (stalkIndex >= nodeCount - 1)
            {
                Debug.LogWarning("Trying to get the next stalk path point, but there are no more!");
                return Vector3.zero;
            }

            return Nodes[stalkIndex + 1];
        }
    }
}