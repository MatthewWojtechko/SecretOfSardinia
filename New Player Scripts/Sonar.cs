using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public LayerMask sonarMask;
    public AudioSource sound;
    public ParticleSystem particleEffect;

    public List<Collider> nearbyPickups = new List<Collider>();

    public float proxIncreaseForEffect = 2f;
    public float lastSqrDistance;
    public float timeLastClockedSpeed;
    public float speedClockRate = 2.5f;

    public Collider currentTarget;

    public Swimput currentInput;

    private float timeEffectPlayed;


    public void Awake()
    {
        FragPickup.onFragFound += OnTriggerExit;
        PlayerHealth.onKill += clearList;
    }

    public void OnDestroy()
    {
        FragPickup.onFragFound -= OnTriggerExit;
        PlayerHealth.onKill -= clearList;
    }

    public void clearList()
    {
        nearbyPickups.Clear();
    }


    public void OnTriggerEnter(Collider col)
    {
        if (!this.enabled)
            return;

        if (sonarMask == (sonarMask | (1 << col.gameObject.layer))) // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        {
            //Debug.Log(col.name);//
            //if (numNearby == 1)
            playEffect();
            if (!nearbyPickups.Contains(col))
            {
                nearbyPickups.Add(col);
                //Debug.Log("COLLIDER ADDED");
            }
            lastSqrDistance = getSqrDist();
            timeLastClockedSpeed = Time.time;
        }
    }

    float getSqrDist()
    {
        if (nearbyPickups.Count == 0)
        {
            currentTarget = null;
            return 0;
        }
        currentTarget = nearbyPickups[0];
        return Vector3.SqrMagnitude(this.transform.position - nearbyPickups[0].transform.position);
    }
    public void OnTriggerExit(Collider col)
    {
        if (!this.enabled)
            return;

        if (sonarMask == (sonarMask | (1 << col.gameObject.layer))) // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        {
            //nearbyPickups.Pop();
            nearbyPickups.Remove(col);
            nearbyPickups.Remove(col);
            lastSqrDistance = getSqrDist();
            timeLastClockedSpeed = Time.time;
        }
    }

    private void playEffect()
    {
        if (!(currentInput.forward || currentInput.backward) || Time.time - timeEffectPlayed < speedClockRate)
            return;

        timeEffectPlayed = Time.time;
        sound.Play();
        particleEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("--------");
        //foreach (Collider c in nearbyPickups)
        //    Debug.Log(c.transform.position);

        if (nearbyPickups.Count == 0)  // None nearby
            return;

        if (Time.time - timeLastClockedSpeed >= speedClockRate) // Check the player's progression toward the object at a certain time interval.
        {
            timeLastClockedSpeed = Time.time;
            float newDistance = getSqrDist();
            float progress = lastSqrDistance - newDistance;
            //Debug.Log(lastSqrDistance + " - " + newDistance);//
            if (progress >= proxIncreaseForEffect)  // If they got sufficiently close, play the effect. Their current distance now becomes what their progress will be based off next.
            {
                playEffect();
                lastSqrDistance = newDistance;
            } 
            else if (newDistance > lastSqrDistance)  // If they didn't make enough progress, and are in fact backtracking, then take their current distance as the new distance to go off of. When they do better than THIS, they'll hear the sound.
            {
                lastSqrDistance = newDistance;
            }
        }

        //float currentSqrDistance = getSqrDist();
        //if (lastSqrDistance - currentSqrDistance >= proxIncreaseForEffect)
        //{
        //    lastSqrDistance = currentSqrDistance;
        //    playEffect();
        //}
    }

    
}
