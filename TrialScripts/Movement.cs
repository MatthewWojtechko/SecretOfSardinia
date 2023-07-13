namespace WaveTrial
{
    using UnityEngine;

    public class Movement : MonoBehaviour
    {
        public float distanceTravelled;
        public Vector3 target;
        float targetDistanceSqr;

        public void setTarget(Vector3 target)
        {
            this.target = target;
            distanceTravelled = 0;
            targetDistanceSqr = Vector3.SqrMagnitude(this.transform.position - target);
            this.transform.LookAt(target);
        }

        // Move toward the target according to given speed.
        // Returns whether we have reached the target or not.
        // Never overshoots target.
        public bool go(float speed)
        {
            float newDistance = speed * TimeKeeper.deltaPlayTime();
            distanceTravelled += newDistance;

            if (distanceTravelled * distanceTravelled < targetDistanceSqr)
            {
                this.transform.position += newDistance * this.transform.forward;
                return false;
            }
            else
            {
                this.transform.position = target;
                return true;
            }
        }
    }
}