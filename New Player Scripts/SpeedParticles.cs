using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem burtParticles;
    public ParticleSystem lostParticles;

    public float emissionOverDistance;
    public float sizePerForce;
    public float magnitudeThreshFactor; // When the player moves at over this * the standard magnitude, start particles
    public bool playing;
    

    private ParticleSystem.EmissionModule particleEmission;
    private ParticleSystem.MainModule particleMain;

    // Start is called before the first frame update
    void Start()
    {
        particleEmission = particles.emission;
        particleMain = particles.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startParticles(float totalForce)
    {
        particles.Play();
        particleEmission.rateOverDistance = emissionOverDistance;
        changeParticleSize(totalForce);

        playing = true;
    }

    public void stopParticles()
    {
        particles.Stop();
        particleEmission.rateOverDistance = 0;

        //Debug.Log(lostParticles);//
        lostParticles.Play();

        playing = false;
    }

    public void changeParticleSize(float totalForce)
    {
        particleMain.startSize = totalForce * sizePerForce;
    }

    public void set(float currentMag, float standardMag)
    {
        if (!playing && currentMag > standardMag * magnitudeThreshFactor)
            startParticles(currentMag);
        else if (playing)
        {
            if (currentMag < standardMag * magnitudeThreshFactor)
                stopParticles();
            else
                changeParticleSize(currentMag);
        }
    }

    public void activateSpeedBoost()
    {
        burtParticles.Play();
    }
}
