using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveBegin : MonoBehaviour
{
    [SerializeField] LevelConstructor levelConstructor;
    [SerializeField] PlayerPhase playerPhase;
    [SerializeField] PhaseEnd phaseEnd;
    [SerializeField] WaveRunner waveRunner;
    
    public void BeginWave(Wave wave)
    {
        StartCoroutine(WaveBeginRoutine(wave));
    }

    IEnumerator WaveBeginRoutine(Wave wave)
    {
        bool finishedLevelConstruction = false;
        levelConstructor.Finished += FinishedLevelConstruction;
        levelConstructor.Run(wave);
        
        while (!finishedLevelConstruction)
        {
            yield return null;
        }
        levelConstructor.Finished -= FinishedLevelConstruction;

        void FinishedLevelConstruction() => finishedLevelConstruction = true;
        waveRunner.PopulateInitialEnemies(wave);
        playerPhase.StartPlayerRound();
        
    }
}
