using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedEffects : MonoBehaviour
{
    public ParticleSystem spirit;

    public void play()
    {
        spirit.Play();
    }
    public void stop()
    {
        spirit.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
