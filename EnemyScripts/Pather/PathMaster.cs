namespace Pathing
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathMaster : MonoBehaviour
    {
        public Path path = new Path();
        public Transform positionPoint;
        public PathState currentState = PathState.NONE;
        private bool turn = false;

        public void Awake()
        {
            path.reset(positionPoint.position);
        }

        //public void resetPath()
        //{
        //    path.reset(positionPoint.position);
        //}


        //public void FixedUpdate()
        //{
        //    if (currentState == PathState.SETTING)
        //    {
        //        if (turn)
        //        {
        //            path.pruneCheck();
        //        }
        //        else
        //        {
        //            path.setCheck(positionPoint.position);
        //        }
        //        turn = !turn;
        //    }
        //}

        // When we head out. Start SETTING the return path.
        public void embarking()
        {
            //path.reset(positionPoint.position);
            currentState = PathState.SETTING;
        }

        // When we start coming back. Start FOLLOWING the return path.
        public void homecoming()
        {
            currentState = PathState.FOLLOWING;
        }

        // When we've made it back.
        public void returned()
        {
            currentState = PathState.NONE;
        }

        public Vector3 getNextReturnPoint()
        {
            return path.getLastNodePosition();
        }

        public bool areWeBackYet()
        {
            return path.getLength() == 1;
        }

        public void reachedNextReturnPoint()
        {
            path.returnedToLastPoint();
        }


        public void OnDrawGizmos()
        {
            if (path.getLength() < 1)
                return;

            Gizmos.color = Color.red;
            Vector3 lastPosition = path.Nodes[0];
            for (int i = 0; i < path.getLength(); i++)
            {
                if (i > 0)
                    Gizmos.DrawLine(path.Nodes[i], lastPosition);
                lastPosition = path.Nodes[i];
                Gizmos.DrawSphere(path.Nodes[i], 0.2f);
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(positionPoint.position, path.getLastNodePosition());
        }

    }

    public enum PathState { NONE, SETTING, FOLLOWING };
}
