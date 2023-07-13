using UnityEngine;
using System;

[System.Serializable]
public class PlayerSpeedManager : MonoBehaviour
{
    [Header("Standard Speed Force")]
    public float regularForwardMag;
    [SerializeField] float slowForwardMag;
    [SerializeField] float regularBackwardMag;
    [SerializeField] float slowBackwardMag;

    [Header("Standard Rotation")]
    [SerializeField] float upDownRotLerp;
    [SerializeField] float leftRightMag;
    [SerializeField] float leftRightRotLerp;
    [SerializeField] float leftRightRotDecel;

    [Header("Slow Rotation")]
    [SerializeField] float upDownRotLerpSLOW;
    [SerializeField] float leftRightMagSLOW;
    [SerializeField] float leftRightRotLerpSLOW;
    [SerializeField] float leftRightRotDecelSLOW;

    [Header("Fast Rotation")]
    public float upDownRotLerpFAST;
    public float leftRightMagFAST;
    public float leftRightRotLerpFAST;
    public float leftRightRotDecelFAST;

    public float currentSpeedPercent;
    public float targetSpeedPercent;
    [Tooltip("In one second, what's the most the speed percent can change? A value of one means it can go from minimum to maximum in a second. A value of 0.5 means it can only increase by half in a second.")]
    public float maxPercentDifferencePerSecond = 1;

    [Space]
    [SerializeField] float maxSpeedHeight = 24;

    // Get Speed Forces
    public float getStandardSpeedForce()
    {
        return getValueScaled(slowForwardMag, regularForwardMag);
    }

    public float getMaxSpeedForce()
    {
        return regularForwardMag;
    }

    public float getBackwardSpeedForce()
    {
        return getValueScaled(slowBackwardMag, regularBackwardMag);
    }

    public float getSlowSpeedForce()
    {
        return slowForwardMag;
    }

    // Get Rotation Lerps
    public float getUpDownRotationLerp()
    {
        return getValueScaled(upDownRotLerpSLOW, upDownRotLerp);
    }
    public float getLeftRightRotationLerp()
    {
        return getValueScaled(leftRightRotLerpSLOW, leftRightRotLerp);
    }
    public float getLeftRightForceMagntiude()
    {
        return getValueScaled(leftRightMagSLOW, leftRightMag);
    }
    public float getLeftRightRotationDecelerationLerp()
    {
        return getValueScaled(leftRightRotDecelSLOW, leftRightRotDecel);
    }

    private float getValueScaled(float lowValue, float highValue)
    {
        return Mathf.Lerp(lowValue, highValue, currentSpeedPercent);
    }

    public void LateUpdate()
    {
        targetSpeedPercent = Mathf.Clamp01(AreaCheck.belowHit_World / maxSpeedHeight);
        float maxDifferencePerFrameTime = TimeKeeper.deltaPlayTime() * maxPercentDifferencePerSecond;
        if (Mathf.Abs(targetSpeedPercent - currentSpeedPercent) > maxDifferencePerFrameTime)  // The change is too extreme. Restrain it.
        {
            if (targetSpeedPercent > currentSpeedPercent)
                currentSpeedPercent += maxDifferencePerFrameTime;
            else
                currentSpeedPercent -= maxDifferencePerFrameTime;
        }
        else
            currentSpeedPercent = targetSpeedPercent;
    }
}