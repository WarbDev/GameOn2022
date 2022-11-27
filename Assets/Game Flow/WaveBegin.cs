using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveBegin : MonoBehaviour
{
    [SerializeField] LevelConstructor levelConstructor;
    public event Action Finished;
    
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
        Finished?.Invoke();

        void FinishedLevelConstruction() => finishedLevelConstruction = true;
    }
}
