using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WaveRunner : MonoBehaviour
{
    [SerializeField] List<Wave> waves;
    [SerializeField] GenericDictionary<ENEMY_TYPE, GameObject> PrefabDictionary;
    [SerializeField] LevelConstructor levelConstructor;

    public event Action Finished;


    List<BatchBase> batchesInWave;
    int currentBatchIndex = 0;
    [SerializeField] int delay = 0;

    // 1-indexed wave number. Wave number 0 is null. Wave number 1 is index 0 in the list of waves.
    [SerializeField] int waveNumber = 0;
    public int WaveNumber { get => waveNumber; }
    [SerializeField] Wave currentWave;

    void Start()
    {
    }

    public Wave GetNextWave()
    {
        waveNumber++;
        if (waveNumber > waves.Count)
        {
            return null;
        }
        currentWave = waves[waveNumber - 1];
        batchesInWave = currentWave.Batches;
        foreach(var batch in batchesInWave)
        {
            batch.ResetBatch();
        }
        return currentWave;
    }

    public bool OnLastWave()
    {
        return waveNumber >= waves.Count;
    }

    public bool NoMoreBatches()
    {
        return currentBatchIndex >= batchesInWave.Count;
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
            batchesInWave[currentBatchIndex].ResetBatch();
            delay += currentWave.Delays[currentBatchIndex];
            currentBatchIndex++;
        }
        Finished?.Invoke();
        
    }
}
