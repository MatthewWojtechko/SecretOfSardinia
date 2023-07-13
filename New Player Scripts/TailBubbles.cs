using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBubbles : MonoBehaviour
{
    public ParticleSystem particles;
    private ParticleSystem.ForceOverLifetimeModule forceMod;

    public float forceFactor = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        forceMod = particles.forceOverLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forceVec = -forceFactor * SardineSwim.instance.body.velocity;
        forceMod.x = forceVec.x;
        forceMod.y = forceVec.y;
        forceMod.z = forceVec.z;
    }
}
