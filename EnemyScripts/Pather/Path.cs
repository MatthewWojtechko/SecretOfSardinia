namespace Pathing
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Path
    {
        public Vector3[] Nodes = new Vector3[100];
        protected int nodeCount = 0;
        Vector3 runningNode;
        protected int searchIndex = 0;

        public virtual void reset(Vector3 position)
        {
            nodeCount = 1;
            Nodes[0] = position;
            searchIndex = 0;
            runningNode = position;
        }

        public int getLength()
        {
            return nodeCount;
        }

        public void prune(int keepToHere)
        {
            if (keepToHere >= nodeCount)
            {
                Debug.LogError("Uh oh! Trying to prune a node out of range: index " + keepToHere + " with a path count of " + nodeCount);
                return;
            }

            nodeCount = keepToHere + 1;
        }

        /*
         * Linecasts between the current (running) node and the node at constantly cycling search index and checks for collision.
         * If there's no collision, we've found a way to simplify (prune) the path - we can remove all nodes after the node at the search index.
         * That node is now the node at the end of the list.
         * This should be called regularly when setting a path so as to keep the path as simple as possible.
         */
        public void pruneCheck()
        {
            //Debug.Log("Path length: " + nodeCount);///
            // Only makes sense to check when we have more than 1 node.
            if (nodeCount < 2)
            {
                //Debug.LogWarning("Whoops! Checking the path even though there's nothing in it yet.");
                return;
            }

            incrementSearchIndex();
            if (!Physics.Linecast(Nodes[searchIndex], runningNode, LayersAndTags.ObstacleMask)) // If no hit between two nodes...
            {
                // ...Then, remove all the nodes after the check node.
                prune(searchIndex);
            }
        }

        public void setCheck(Vector3 currentPosition)
        {
            // Only makes sense to check once the path has started (has at least one node).
            if (nodeCount < 1)
            {
                return;
            }


            if (Physics.Linecast(getLastNodePosition(), currentPosition, LayersAndTags.ObstacleMask)) // If there's a collision between two nodes...
            {
                // ...we add the running node to the path.
                setNewNode(currentPosition);
            }

            // The running node is set to the current position.
            runningNode = currentPosition;
        }

        void setNewNode(Vector3 position)
        {
            if (nodeCount >= Nodes.Length)
            {
                Debug.LogWarning("Path is at the maximum number of nodes. Can't add any more! The enemy should turn back now. Otherwise, program him with a bigger path capacity. \"Capathity,\" if you will. lol");
                return;
            }

            Nodes[nodeCount] = position;
            nodeCount++;
        }

        public Vector3 getLastNodePosition()
        {
            if (nodeCount == 0)
            {
                Debug.LogError("Attempting to access the last node, but there are no nodes!");
                return Vector3.zero;
            }

            return Nodes[nodeCount - 1];
        }

        protected virtual void incrementSearchIndex()
        {
            searchIndex++;
            if (searchIndex >= nodeCount)
                searchIndex = 0;
        }

        public void returnedToLastPoint()
        {
            nodeCount--;
            if (nodeCount < 1) // Always keep the "home" node.
            {
                nodeCount = 1;
            }
        }
    }
}
