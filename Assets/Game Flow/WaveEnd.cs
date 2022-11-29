using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnd : MonoBehaviour
{
    [SerializeField] WaveRunner waveRunner;
    [SerializeField] WaveBegin waveBegin;
    [SerializeField] WaveWin waveWin;
    [InspectorButton("RunWaveEnd")]
    public bool Run;

    public void RunWaveEnd(int waveNumber)
    {
        waveWin.WaveWon(waveNumber);
        waveBegin.BeginWave(waveRunner.GetNextWave());
    }

    private void Start()
    {
        waveBegin.BeginWave(waveRunner.GetNextWave());
    }
}
