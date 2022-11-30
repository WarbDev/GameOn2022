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
    [SerializeField] AudioClip music_A;
    [SerializeField] AudioClip music_B;
    [SerializeField] AudioSource audioSource;

    [SerializeField] int BMusicThreshold = 2;
    public void BeginWave(Wave wave)
    {
        StartCoroutine(WaveBeginRoutine(wave));
    }

    IEnumerator WaveBeginRoutine(Wave wave)
    {
        audioSource.clip = GetAppropriateMusic(waveRunner.WaveNumber);
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
        audioSource.Play();
        
    }

    AudioClip GetAppropriateMusic(int waveNumber)
    {
        if (waveNumber >= BMusicThreshold)
        {
            return music_B;
        }
        else return music_A;
    }
}
