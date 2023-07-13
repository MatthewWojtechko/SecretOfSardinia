using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetSardine : MonoBehaviour
{
    //public PlayerDeath death;
    public Transform camMan;
    public Vector3 position;
    public Quaternion rotation;
    public SardineSwim swim;
    public ParticleSystem spawnPart;
    public float spawnDuration = 3;
    public float spawnFormDelay = 1;
    public Animator anim;


    float startTime = 0;

    public static event Action onRespawn;
    public static event Action onEndRespawn;

    //public void Start()
    //{
    //    resetSardine();
    //}

    //public void resetSardine()
    //{
    //    this.transform.position = position;
    //    camMan.position = position;
    //    this.transform.rotation = rotation;
    //    camMan.rotation = rotation;
    //    foreach (Behaviour S in death.scriptsToKill)
    //    {
    //        S.enabled = true;
    //    }
    //    death.skin.enabled = true;
    //    swim.enabled = false;
    //    swim.resetRotation(Vector3.zero);

    //    PlayerHealth.revive();
    //    //FishSoundController.respawningStart();
    //}

    //public void spawnIn()
    //{
    //    // Reset animator parameters
    //    anim.SetFloat("Forward", 1);
    //    anim.SetFloat("joyX", 0);
    //    anim.SetFloat("joyY", 0);
    //    spawnPart.Play();
    //    onRespawn?.Invoke();
    //    StartCoroutine(waitSpawnControl());
    //}

    //IEnumerator waitSpawnControl()
    //{
    //    startTime = Time.time;
    //    PlayerHealth.updateDissolveManually(0);
    //    yield return new WaitUntil(() => Time.time - startTime > spawnFormDelay);

    //    while (Time.time - startTime <= spawnDuration)
    //    {
    //        PlayerHealth.updateDissolveManually((Time.time - startTime - spawnFormDelay) / spawnDuration);
    //        yield return new WaitForEndOfFrame();
    //    }
    //    PlayerHealth.updateDissolveManually(1);
    //    //yield return new WaitForSeconds(spawnDuration);
    //    onEndRespawn?.Invoke();
    //    swim.enabled = true;
    //}

}