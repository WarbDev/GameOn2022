using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveRunner : MonoBehaviour
{
    [SerializeField] Wave waveToRun;
    [SerializeField] GenericDictionary<ENEMY_TYPE, GameObject> PrefabDictionary;
    [SerializeField] EnemyPhase enemyPhase;

    List<BatchBase> batchesInWave;
    int currentBatchIndex = 0;
    [SerializeField] int delay = 0;

    void Start()
    {
        StartCoroutine(WaitRun());
        batchesInWave = waveToRun.Batches;

        enemyPhase.Finished += RunNextBatch;
    }

    IEnumerator WaitRun()
    {
        yield return new WaitForSeconds(2);
        waveToRun.InitializeWave();
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
