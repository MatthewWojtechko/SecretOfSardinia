using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedInterpolation : MonoBehaviour
{
    public EnemyController Controller;
    public bool isActive = true;
    public AnimationCurve curve;
    public float pointBlankDistance = 2;

    public float interpolate(float proposedSpeed)
    {
        if (!isActive || Controller.Behavior.currentState != Controller.Behavior.CHASE)
            return proposedSpeed;

        float playerDistanceSqr = Controller.Look.getPlayerDistanceSqr();
        float distancePercent = Mathf.Clamp01((playerDistanceSqr - Mathf.Pow(pointBlankDistance, 2)) / (Mathf.Pow(Controller.Look.getCurrentSightDistance(), 2) - (Mathf.Pow(pointBlankDistance, 2))));
        return curve.Evaluate(distancePercent) * proposedSpeed;
    }
}
