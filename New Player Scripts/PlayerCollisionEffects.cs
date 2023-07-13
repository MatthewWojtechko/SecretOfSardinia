using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionEffects : MonoBehaviour
{
    public GameObject[] collisionGOs;
    public ParticleSystem[] particles;
    public ImpactSoundController[] sounds;
    public float[] startTimes;
    public float particleDuration = 0.5f;
    public float effectCooldown = 0.2f;
    private float lastEffectTime = -10f;



    public float magnitudeThreshold = 0.2f;
    public float maxThreshold = 2f;
    public float minScale = 1;
    public float maxScale = 4;

    public SardineSwim swimScript;

    public LayerMask effectLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (effectLayer == (effectLayer | (1 << collision.collider.gameObject.layer)) && Time.time - lastEffectTime >= effectCooldown)
        {
            lastEffectTime = Time.time;
            float mag = SardineSwim.playerRigid.velocity.magnitude;
            //Debug.Log("COLLISION: " + SardineSwim.playerRigid.velocity.magnitude);//
            if (mag > magnitudeThreshold)
                choosePrefab(collision.GetContact(0).point, collision.GetContact(0).normal, mag);
        }
    }

    // Goes through all the effects, and finds the first one that is playable.
    // Prefabs are playable if the time when they were last played has been long enough ago.
    // When the prefab is found, it plays it using the activateEffect function and exits.
    // If it goes through all effects and does not find a free one, then it does not play one.
    void choosePrefab(Vector3 position, Vector3 normal, float mag)
    {
        //Debug.Log("choose");
        for (int i = 0; i < collisionGOs.Length; i++)
        {
            if (Time.time - startTimes[i] > particleDuration)
            {
                activateEffect(i, position, normal, mag);
                break;
            }
        }
    }

    void activateEffect(int index, Vector3 position, Vector3 normal, float mag)
    {
        //Debug.Log("place");
        if (collisionGOs[0] == null)
        {
            //Debug.LogWarning("Insufficient collision gameobject found in pool references.");//
            return;
        }
        collisionGOs[index].transform.up = normal;//SardineSwim.playerTransform.up * -1;
        collisionGOs[index].transform.position = position;
        float impactPercent = Mathf.Clamp01((mag - magnitudeThreshold) / (maxThreshold - magnitudeThreshold));
        collisionGOs[index].transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, impactPercent);
        particles[index].Play(); // Play sounds
        if (mag >= maxThreshold)
            sounds[index].playHeavy(impactPercent);
        else
            sounds[index].playLight(impactPercent);
        startTimes[index] = Time.time;
    }


}
