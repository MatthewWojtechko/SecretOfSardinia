/*
 * Modifies certain characteristics of player character when they have won certain abilities.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bestow : MonoBehaviour
{
    public MonoBehaviour[] abilities;
    public BoundaryController boundary;

    private static Bestow instance;

    public void awakenEarnedAbilities()
    {
        int numEarned = SaveDataManager.trials.getNumRewardsEarned(7);

        // Unlock any of the 3 gifts
        if (numEarned >= 1)
            abilities[0].enabled = true;
        if (numEarned >= 3)
            abilities[1].enabled = true;
        if (numEarned >= 6)
            abilities[3].enabled = true;

        // Set the max height to any of the four altitudes
        if (numEarned >= 7)
            boundary.setHeight(4);
        else if (numEarned >= 5)
            boundary.setHeight(3);
        else if (numEarned >= 4)
            boundary.setHeight(2);
        else if (numEarned >= 2)
            boundary.setHeight(1);
        else
            boundary.setHeight(0);
    }

    private void Start()
    {
        //awakenEarnedAbilities();
    }
}
