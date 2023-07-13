using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialWarningPart : MonoBehaviour
{
    public ParticleSystem part;
    static TrialWarningPart instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void play()
    {
        instance.part.Play();
    }

    public static void stop()
    {
        instance.part.Play();
        instance.part.Stop();
    }

    public static void freeze()
    {
        instance.part.Pause();
    }
}
