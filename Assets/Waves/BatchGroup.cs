using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a collection of batches, which must all be completed before finishing.
/// </summary>
[CreateAssetMenu(fileName = "Batch Group", menuName = "ScriptableObjects/BatchGroup")]
public class BatchGroup : BatchBase
{
    [SerializeField] List<BatchBase> batches = new();
    int currentBatch = 0;
    public override void ResetBatch()
    {
        currentBatch = 0;
        foreach(var batch in batches)
        {
            batch.ResetBatch();
        }
    }

    public override Dictionary<Location, ENEMY_TYPE> RunBatch(out bool isFinished)
    {
        var popDictionary = batches[currentBatch].RunBatch(out bool isIndividualBatchFinished);
        if (isIndividualBatchFinished)
        {
            currentBatch++;
            batches[currentBatch].ResetBatch();
        }
        isFinished = AtEndOfBatches();
        return popDictionary;
    }

    bool AtEndOfBatches()
    {
        return currentBatch >= batches.Count;
    }
}