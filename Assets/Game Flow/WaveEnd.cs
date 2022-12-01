using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnd : MonoBehaviour
{
    [SerializeField] WaveRunner waveRunner;
    [SerializeField] WaveBegin waveBegin;
    [SerializeField] WaveWin waveWin;
    [SerializeField] LevelConstructor levelConstructor;

    [InspectorButton("RunWaveEnd")]
    public bool Run;

    public void RunWaveEnd(int waveNumber)
    {
        bool needsRestart = false;

        // waveWin.RestartWave += setRestart
        levelConstructor.Finished += onDeconstructed;
        levelConstructor.Derun(waveRunner.currentWave);
        

        void onDeconstructed()
        {
            levelConstructor.Finished -= onDeconstructed;
            waveWin.WaveWon(waveNumber);
            waveWin.onWinScreenDismissed += onWinScreenDismissed;
        }

        void onWinScreenDismissed()
        {
            waveWin.onWinScreenDismissed -= onWinScreenDismissed;
            // waveWin.RestartWave -= setRestart

            if (needsRestart)
            {
                // waveBegin.BeginWave(waveRunner.GetSameWave());
            }
            else if (!waveRunner.OnLastWave())
            {
                waveBegin.BeginWave(waveRunner.GetNextWave());
            }
        }

        void setRestart()
        {
            needsRestart = true;
        }
    }

    private void Start()
    {
        waveBegin.BeginWave(waveRunner.GetNextWave());
    }
}
