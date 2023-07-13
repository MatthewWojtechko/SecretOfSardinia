using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerSwimmingEffects : MonoBehaviour
{
    public SardineSwim swimScript;
    public ParticleSystem boostParticles;

    // Particles used for the trail effect when propelled
    // Elements 0 and 1 should be the cloud particles
    public ParticleSystem[] trailParticles;
    public float[] gasMaxPartSizeRange = { 0.8f, 1.4f };  // Maximums for: min start size, max start size
    public float sparklesMaxRate = 6;
    public float sparklesMaxSpeed = 12.53f;
    public float sparklesMinSpeed = 5;
    public float threshold = 10f;   // Need to be moving this musch faster than normal to see any trail effect
    public float maxExcess = 100;   // Effect scales up to this amount. When going faster than this, effect is at its extreme.
    public float thresholdForFOV = 30;
    public float propulsionFOV = 60;
    public float FOVChangeTime;
    private float propStartTime;
    private float propEndTime;
    private float propEndFOV = 60;
    private float revertEndFOV = VirtualCameraManager.standardFOV;
    private float fovStartTime, fovEndTime;

    public Volume volume;
    private ChromaticAberration chromAberr;
    public float chromAberrStandard = 0.053f;
    public float chromAberrMax = 1;
    private float propEndChromAberr;
    private float revertEndChromAber = 0.053f;

    private bool isTrailPlaying = false;
    private bool isFOVincreasePlaying = false;
    ParticleSystem.MinMaxCurve startSize1;
    //ParticleSystem.MinMaxCurve startSize2;
    ParticleSystem.MainModule particleMod1;
    ParticleSystem.MainModule particleMod2;
    ParticleSystem.MainModule particleMod3;
    ParticleSystem.EmissionModule emission3;

    private void Awake()
    {
        Skim.onBoost += boostPartPlay;
    }
    private void OnDestroy()
    {
        Skim.onBoost -= boostPartPlay;
    }

    void boostPartPlay()
    {
        boostParticles.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        startSize1 = trailParticles[0].main.startSize;
        //startSize2 = trailParticles[1].main.startSize;

        particleMod1 = trailParticles[0].main;
        particleMod2 = trailParticles[1].main;

        particleMod3 = trailParticles[2].main;
        emission3 = trailParticles[2].emission;

        volume.profile.TryGet(out chromAberr);  // NOT WORKING
    }

    public void Update()
    {
        propelled(swimScript.compositeMagnitude - swimScript.Speed.getStandardSpeedForce());
    }

    // Turn trailParticles on/off
    // Compare current magnitude to regular forward magnitude
    // Start effects at a certain threshold over this second value
    // The strength of the effects start small, at 0, and scale in size up to a given maximum difference between current magnitude and reg forward mag
    private void propelled(float excessMagnitude)
    {
        bool isPropelled = excessMagnitude > threshold;
        //bool isBigFOVNeeded = excessMagnitude > thresholdForFOV;

        if (isTrailPlaying) // If effect currently playing
        {
            if (!isPropelled) // Fish not going fast enough for effect, so STOP the effect
            {
                isTrailPlaying = false;
                propEndTime = Time.time;
                for (int i = 0; i < trailParticles.Length; i++)
                {
                    trailParticles[i].Stop();
                }
            }
            else // update the effect
            {
                updateParticleSettings(excessMagnitude);
            }
        }
        else if (isPropelled) // Else if effect not playing, but should be, then start it.
        {
            isTrailPlaying = true;
            propStartTime = Time.time;
            for (int i = 0; i < trailParticles.Length; i++)
            {
                trailParticles[i].Play();
            }
        }


       //FOVmodifier.propulsion(isBigFOVNeeded);

        //// FIELD OF VIEW
        //if (isFOVincreasePlaying) // If FOV is increasing
        //{
        //    if (!isBigFOVNeeded) // But a big FOV is not needed
        //    {
        //        isFOVincreasePlaying = false;
        //        fovEndTime = Time.time;
        //        revertEndFOV = propEndFOV;
        //    }
        //    else
        //    {
        //        increaseFOV();
        //    }
        //}
        //else if (isBigFOVNeeded) // Else FOV increase is not playing, but it is needed
        //{
        //    isFOVincreasePlaying = true;
        //    fovStartTime = Time.time;
        //}
        //else // Else the FOV increase has ended and is not needed
        //{
        //    revertFOV();
        //}
    }

    void updateParticleSettings(float currentExcess)
    {
        float lerpConstant = Mathf.Clamp01(currentExcess / maxExcess);
        //Debug.Log("Lerp Constant: " + lerpConstant);

        startSize1.constantMin = Mathf.Lerp(0, gasMaxPartSizeRange[0], lerpConstant);
        startSize1.constantMax = Mathf.Lerp(0, gasMaxPartSizeRange[1], lerpConstant);

        //startSize2.constantMin = Mathf.Lerp(0, gasMaxPartSizeRange[0], lerpConstant);
        //startSize2.constantMax = Mathf.Lerp(0, gasMaxPartSizeRange[1], lerpConstant);

        particleMod1.startSize = startSize1;
        particleMod2.startSize = startSize1;

        particleMod3.startSpeed = Mathf.Lerp(sparklesMinSpeed, sparklesMaxSpeed, lerpConstant);
        emission3.rateOverDistance = Mathf.Lerp(0, sparklesMaxRate, lerpConstant);
    }

    //void increaseFOV()
    //{
    //    //Debug.Log(revertEndFOV);//
    //    float lerpConstant = Mathf.Clamp01((Time.time - fovStartTime) / FOVChangeTime);

    //    // Modify camera's field of view
    //    float newFOV = Mathf.Lerp(revertEndFOV, propulsionFOV, lerpConstant);
    //    propEndFOV = newFOV;
    //    VirtualCameraManager.setFOV(newFOV);

    //    float newChAber = Mathf.Lerp(revertEndChromAber, chromAberrMax, lerpConstant);
    //    propEndChromAberr = newChAber;
    //    chromAberr.intensity.value = newChAber;
    //}

    //void revertFOV()
    //{
    //    if (revertEndFOV > VirtualCameraManager.standardFOV)
    //    {
    //        float lerpConstant = Mathf.Clamp01((Time.time - fovEndTime) / FOVChangeTime);

    //        float newFOV = Mathf.Lerp(propEndFOV, VirtualCameraManager.standardFOV, lerpConstant);
    //        revertEndFOV = newFOV;
    //        VirtualCameraManager.setFOV(newFOV);

    //        float newChAber = Mathf.Lerp(propEndChromAberr, chromAberrStandard, lerpConstant);
    //        revertEndChromAber = newChAber;
    //        chromAberr.intensity.value = newChAber;
    //    }
    //}
}
