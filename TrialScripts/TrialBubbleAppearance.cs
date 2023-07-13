using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialBubbleAppearance : MonoBehaviour
{
    public Transform bubble;
    public float maxScale = 0.2f;
    private float expandSeconds = 0.1f;
    private float shrinkSeconds = 0.1f;
    public ParticleSystem unwonparticles;


    private float startScale;
    private float startTime;
    private float seconds;
    private float target;
    private bool active = false;

    public void setHasWon()
    {
        unwonparticles.Stop();
    }


    public void expand()
    {
        startScale = bubble.localScale.x;
        startTime = Time.time;
        target = maxScale;
        seconds = shrinkSeconds;

        active = true;
    }

    public void shrink()
    {
        startScale = bubble.localScale.x;
        startTime = Time.time;
        target = 0;
        seconds = shrinkSeconds;

        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            float lerp = Mathf.Clamp01((Time.time - startTime) / seconds);
            bubble.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * target, lerp);
            active = bubble.localScale.x != (Vector3.one * target).x;
        }
    }
}
