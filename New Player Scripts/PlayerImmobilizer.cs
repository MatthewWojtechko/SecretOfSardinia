using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveTrial;

public class PlayerImmobilizer : MonoBehaviour
{
    public bool isWinningTrial = false;

    public void Awake()
    {
        WinState.onTrialNewlyWon += trialWon;
        TrialWinController.onTrialNewlyWonEnd += trialWinEnd;
    }

    public void OnDestroy()
    {
        WinState.onTrialNewlyWon -= trialWon;
        TrialWinController.onTrialNewlyWonEnd -= trialWinEnd;
    }

    void trialWon()
    {
        isWinningTrial = true;
    }
    void trialWinEnd()
    {
        isWinningTrial = false;
    }

    public bool isPlayerImmobile()
    {
        return isWinningTrial;
    }
}
