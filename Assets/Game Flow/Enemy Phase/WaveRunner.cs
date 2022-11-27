using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WaveRunner : MonoBehaviour
{
    [SerializeField] Wave waveToRun;
    [SerializeField] GenericDictionary<ENEMY_TYPE, GameObject> PrefabDictionary;
    [SerializeField] LevelConstructor levelConstructor;

    public event Action Finished;


    List<BatchBase> batchesInWave;
    int currentBatchIndex = 0;
    [SerializeField] int delay = 0;

    [SerializeField] Wave currentWave;

    void Start()
    {
        batchesInWave = waveToRun.Batches;
    }

    public Wave GetNextWave()
    {
        return currentWave;
    }

    public void PopulateInitialEnemies(Wave wave)
    {
        foreach (var kvp in wave.initialEnemies.GetInitialEnemies())
        {
            Entities.SpawnEnemy(kvp.Key, PrefabDictionary[kvp.Value]);
        }
    }

    public void RunNextBatch()
    {
        if (delay > 0)
        {
            delay -= 1;
            Finished?.Invoke();
            return;
        }

        if (batchesInWave.Count <= currentBatchIndex)
        {
            Finished?.Invoke();
            return;
        }

        var batch = batchesInWave[currentBatchIndex].RunBatch(out bool isBatchFinished);
        foreach (var kvp in batch)
        {
            Entities.SpawnEnemy(kvp.Key, PrefabDictionary[kvp.Value]);
        }

        if (isBatchFinished)
        {
            delay += waveToRun.Delays[currentBatchIndex];
            currentBatchIndex++;
        }
        Finished?.Invoke();
        
    }
}
