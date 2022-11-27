using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnd : MonoBehaviour
{
    [SerializeField] WaveRunner waveRunner;
    [SerializeField] WaveBegin waveBegin;
    [InspectorButton("RunWaveEnd")]
    public bool Run;

    private void RunWaveEnd()
    {
        waveBegin.BeginWave(waveRunner.GetNextWave());
    }

    private void Start()
    {
        waveBegin.BeginWave(waveRunner.GetNextWave());
    }
}
