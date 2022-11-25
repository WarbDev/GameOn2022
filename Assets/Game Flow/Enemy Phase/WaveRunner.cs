using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveRunner : MonoBehaviour
{
    [SerializeField] Wave waveToRun;
    [SerializeField] GenericDictionary<ENEMY_TYPE, GameObject> PrefabDictionary;

    List<BatchBase> batchesInWave;
    int currentBatchIndex = 0;
    [SerializeField] int delay = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            waveToRun.InitializeWave();
            batchesInWave = waveToRun.Batches;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            RunNextBatch();
        }
    }

    void RunNextBatch()
    {
        if (delay > 0)
        {
            delay -= 1;
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
        
    }
}
