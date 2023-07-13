using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightLineManager : MonoBehaviour
{
    public GameObject lineGameObject;

    public LineRenderer line;
    public Transform myEyes;

    public float[] lineStartWidth = { 0.001f, 0.01f };
    public float[] lineEndWidth = { 0.001f, 0.015f };
    public float[] lineEmission = { 0, 407 };
    public float lineEmissionTime = 1;

    public Color lineColorAttack = Color.red;

    private Material lineMaterial;

    private float timeDifference;
    private float startWidthDifference;
    private float endWidthDifference;
    private float lineEmissionDifference;
    private float lineEmissionStopWatch = 0;

    private float testTime;

    // Start is called before the first frame update
    void Start()
    {
        lineMaterial = line.material;
        startWidthDifference = lineStartWidth[1] - lineStartWidth[0];
        endWidthDifference = lineEndWidth[1] - lineEndWidth[0];
        lineEmissionDifference = lineEmission[1] - lineEmission[0];
    }

    /*
     * Call before you make the effect visible.
     */
    public void prepare()
    {
        //lineGameObject.transform.parent = SardineSwim.playerTransform;
        //lineGameObject.transform.localPosition = Vector3.zero;
    }

    /*
     * When given how long the warning is, and how long the warning has been active, this method sizes the line accordingly.
     */
    public void warning(float currentTime, float maxTime)
    {
        lineMaterial.SetColor("_EmissionColor", Color.white);
        //PlayerHitParticles.stop(this);

        if (currentTime > maxTime)
            timeDifference = 1;
        else
            timeDifference = (currentTime / maxTime);

        line.startWidth = (timeDifference * startWidthDifference) + lineStartWidth[0];
        line.endWidth = (timeDifference * endWidthDifference) + lineEndWidth[0];

        lineEmissionStopWatch = 0;
        // set as false when appropriate
    }

    public void firstSee()
    {
        //PlayerHitParticles.firstAttack(this);
    }

    /*
     * Sets the line for proper thickness and emission when in the see/attack state.
     * Automatically lerps the emission from none to the color provided in the global field.
     * The line width is set proportionally to the given distancePercent.
     * The distance percent is how far the sardine is from this enemy, divided by how far they CAN be while still being in sight.
     */
    public void attack(float distancePercent)
    {
        //PlayerHitParticles.attack(currentDistance, maxDistance);

        line.startWidth = lineStartWidth[1];
        line.endWidth = ((1 - distancePercent) * endWidthDifference) + lineEndWidth[0];

        //line.gameObject.layer = glowLayer;
        lineEmissionStopWatch += Time.deltaTime;
        if (lineEmissionStopWatch > lineEmissionTime)
            lineEmissionStopWatch = lineEmissionTime;
        lineMaterial.SetColor("_EmissionColor", (((lineEmissionStopWatch / lineEmissionTime) * lineEmissionDifference) + lineEmission[0]) * lineColorAttack);
    }

    /*
     * When the warning or attack state end. Turn everything off.
     */
    public void lostSight()
    {
        //hitParticles.transform.parent = null;
        //PlayerHitParticles.stop(this);
        line.startWidth = line.endWidth = 0;
        off();
        //line.gameObject.layer = defaultLayer;
        //lineMaterial.SetColor("_EmissionColor", Color.black);
        //lineGameObject.transform.parent = this.transform;
    }

    public void off()
    {
        if (line.enabled)
            line.enabled = false;
    }

    public void on()
    {
        if (!line.enabled)
            line.enabled = true;
    }

    public void updatePosition()
    {
        line.SetPosition(0, myEyes.position);
        line.SetPosition(1, SardineSwim.playerTransform.position);
    }

    //public void updatePositionObstructed(Vector3 lastSeenAt)
    //{
    //    line.SetPosition(0, myEyes.position);
    //    line.SetPosition(1, lastSeenAt);
    //}
}
