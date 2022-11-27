using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveBegin : MonoBehaviour
{
    [SerializeField] LevelConstructor levelConstructor;
    [SerializeField] EnemyPhase enemyPhase;
    
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
        enemyPhase.StartRound();
        void FinishedLevelConstruction() => finishedLevelConstruction = true;
    }
}
