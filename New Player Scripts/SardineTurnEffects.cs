using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SardineTurnEffects : MonoBehaviour
{
    public InputParticles.Effect turnEffect;

    private ParticleSystem.ForceOverLifetimeModule forceModTurn;
    public float turnForce = 1;

    private bool verticalFlickPrev = false;
    private bool horizontalFlickPrev = false;

    public Swimput currentInput;

    public static event Action onTurn;
    public static event Action onDive;

    public void Start()
    {
        forceModTurn = turnEffect.particles.forceOverLifetime;
    }

    public void Awake()
    {
        SardineSwim.onUpdateComplete += updateParticles;
    }

    public void OnDestroy()
    {
        SardineSwim.onUpdateComplete -= updateParticles;
    }

    // CALL THIS ON SWIM UPDATE COMPLETE!!!
    public void updateParticles()
    {
        // If just flicked
        if (!verticalFlickPrev && (currentInput.flickDown() || currentInput.flickUp()))// vertical
        {
            if (currentInput.flickDown() && currentInput.forward)
            {
                onDive?.Invoke();
            }
            else
            {
                onTurn?.Invoke();
            }

            forceModTurn.x = 0;
            forceModTurn.z = 0;
            forceModTurn.y = turnForce * currentInput.rotation.y;
            InputParticles.play(ref turnEffect);
        }
        else if (!horizontalFlickPrev && (currentInput.flickLeft() || currentInput.flickRight())) // horizontal
        {
            onTurn?.Invoke();

            forceModTurn.y = 0;
            forceModTurn.z = 0;
            forceModTurn.x = -turnForce * currentInput.rotation.x;
            InputParticles.play(ref turnEffect);
        }

        verticalFlickPrev = currentInput.flickUp() || currentInput.flickDown();
        horizontalFlickPrev = currentInput.flickLeft() || currentInput.flickRight();
    }
}
