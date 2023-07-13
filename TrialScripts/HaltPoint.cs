namespace WaveTrial
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HaltPoint : Waypoint
    {
        public float duration = 5;
        public Standing placement = Standing.MIDDLE;

        public enum Standing { FIRST, MIDDLE, LAST };
        public override void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, 0.15f);
        }
    }
}