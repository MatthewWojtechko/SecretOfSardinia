/*
 * This script creates a floaty-drifty semi-random movement to the transform to simulate floating under the water.
 * 
 * There are two modes of movement.
 * 
 * One is through perlin noise, which bobs the transform back and forth in the same general direction. 
 * This is movement eases in and out, but it looks to predictable and samey on its own. 
 * 
 * The other movement mode picks a random point in a sphere, and moves the transform toward it over time.
 * It doesn't move toward the random point uniformly - it eases in at the start, speeds up, then eases as it arrives at the destination.
 * This is implemented by using the cubic-bezier mathematical function - graph it to see how it eases in and out.
 * (Each move to the new target takes a completely random amount of time, regardless of how much distance there is to travel.
 * Distance to travel is not calculated so as to save on CPu usage in the event of using this script multiple times - a more graceful 
 * floating effect may instead take it into account, so small distances take less time and longer ones take more time, proportionately, with 
 * some randomness thrown in.)
 * 
 * Both modes are combined through a simple addition to the transform's parent's position - which should be static.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialBubbleDrift : MonoBehaviour
{
    // Range over which height varies.
    public float scale = 1.0f;

    // Distance covered per second along X axis of Perlin plane.
    public float xAxisSpeed = 1.0f;
    public float yAxisSpeed = 1.0f;
    public float zAxisSpeed = 1.0f;

    public float randomRange = 5;
    private float randomXScale, randomYScale, randomZScale;


    public float randomPointRadius = 1;
    public float randomPointTimeRangeMin = 0.5f;
    public float randomPointTimeRangeMax = 1;
    private float targetReachDuration;
    private float targetFloatStartedAt;
    private Vector3 posStart;
    private Vector3 randomTarget = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        randomXScale = Random.Range(-randomRange, randomRange);
        randomYScale = Random.Range(-randomRange, randomRange);
        randomZScale = Random.Range(-randomRange, randomRange);
        getNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        float xAddition = randomXScale * Mathf.PerlinNoise(Time.time * xAxisSpeed, 0);
        float yAddition = randomYScale * Mathf.PerlinNoise(Time.time * yAxisSpeed, 0);
        float zAddition = randomZScale * Mathf.PerlinNoise(Time.time * zAxisSpeed, 0);
        Vector3 noiseAddition = new Vector3(xAddition, yAddition, zAddition);

        if (Time.time > targetFloatStartedAt + targetReachDuration)
            getNewTarget();
        Vector3 randomFloatAddition = getNewSpherePoint();

        //Debug.Log(this.transform.parent.position + " " + noiseAddition + " " + randomFloatAddition);//
        this.transform.position = this.transform.parent.position + noiseAddition + randomFloatAddition;
    }

    void getNewTarget()
    {
        posStart = randomTarget;
        randomTarget = Random.insideUnitSphere * randomPointRadius;
        targetFloatStartedAt = Time.time;
        targetReachDuration = Random.Range(randomPointTimeRangeMin, randomPointTimeRangeMax);
    }

    Vector3 getNewSpherePoint()
    {
        float lerp = (Time.time - targetFloatStartedAt) / targetReachDuration;
        lerp = cubicBezier(lerp);
        return Vector3.Slerp(posStart, randomTarget, lerp);
    }

    float cubicBezier(float x)
    {
        return 3 * Mathf.Pow(x, 2) - 2 * Mathf.Pow(x, 3);
    } // https://math.stackexchange.com/questions/26846/is-there-an-explicit-form-for-cubic-b%C3%A9zier-curves
}
