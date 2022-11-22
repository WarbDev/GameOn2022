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
    int delay = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            batchesInWave = waveToRun.BatchesWithDelays.Keys.ToList();
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



        bool canRun = batchesInWave[currentBatchIndex].TryRunBatch(out Dictionary<Location, ENEMY_TYPE> batch);
        if (canRun)
        {
            foreach(var kvp in batch)
            {
                Entities.SpawnEnemy(kvp.Key, PrefabDictionary[kvp.Value]);
            }
            delay += waveToRun.BatchesWithDelays[batchesInWave[currentBatchIndex]];
            currentBatchIndex++;
        }
    }
}
