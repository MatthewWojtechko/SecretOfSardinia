using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSardine : MonoBehaviour
{
    // Components and names for fading
    public string propertyName;
    //public Material fadeMaterial;
    //public Material regularMaterial;
    public Renderer myRend;

    // Controls for the partial fading effect and disappearing effect
    public float fadeStartNum = 2;
    public float partialFadeEndNum = 0.15f;
    public float disappearFadeEndNum = 0;
    public float currentOpacity;

    public float fadeLerpConst = 0.3f;
    public float fadeLerpThreshold = 0.01f;

    private float opacityBeforeOracle;
    public bool inOracle;

    public Outline outline;

    public float outlineWidth = 0.2f;

    public ParticleSystem bubbles;
    public Collider[] colliders;

    public void Start()
    {
        OraclePlayerControl.onOracleBegun += oracleFadeOut;
        OraclePlayerControl.onOracleExit += oracleFadeIn;
    }
    public void OnDestroy()
    {
        OraclePlayerControl.onOracleBegun -= oracleFadeOut;
        OraclePlayerControl.onOracleExit -= oracleFadeIn;
    }

    void oracleFadeOut()
    {
        opacityBeforeOracle = currentOpacity;
        LeanTween.value(this.gameObject, dither, currentOpacity, 1, 1);
        inOracle = true;

        ceaseToExist();
    }
    void oracleFadeIn()
    {
        outline.enabled = true;
        LeanTween.value(this.gameObject, dither, 1, opacityBeforeOracle, 1).setOnComplete(() => inOracle = false);

        resumeToExist();
    }

    public void publicDither(float amnt)
    {
        myRend.material.SetFloat(propertyName, amnt * fadeStartNum);
        currentOpacity = amnt;
        setLine(amnt);
    }

    void dither(float target)
    {
        // 1 - is faded
        // 0 - is visible
        myRend.material.SetFloat(propertyName, target * fadeStartNum);
        //Debug.Log("GET: " + myRend.material.GetFloat(propertyName));//

        setLine(target);
    }

    void setLine(float target)
    {
        outline.OutlineWidth = (1 - target) * outlineWidth;
        if (outline.OutlineWidth == 0)
            outline.enabled = false;
        else if (!outline.enabled)
            outline.enabled = true;

    }

    void ceaseToExist()
    {
        bubbles.Stop();
        foreach (Collider C in colliders)
            C.enabled = false;
    }
    void resumeToExist()
    {
        bubbles.Play();
        foreach (Collider C in colliders)
            C.enabled = true;
    }
}
