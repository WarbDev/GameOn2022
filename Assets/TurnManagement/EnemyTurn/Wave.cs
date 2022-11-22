using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data container for defining the parameters of an individual wave, such as the map's
/// boundaries, and also vitally contains a collection of groups of enemies, called "batches".
/// </summary>
[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    [Range(3, 8)][SerializeField] int height;
    [Range(1, 16)][SerializeField] int rightLength;
    [Range(0, 16)][SerializeField] int leftLength;

    [Tooltip("Represents a set of batches and how many turns until the succeeding batch attempts to run.")]
    [SerializeField] GenericDictionary<BatchBase, int> batchesAndTurnDelays;

    public GenericDictionary<BatchBase, int> BatchesWithDelays { get => batchesAndTurnDelays; }
    public int Height { get => height; }
    public int RightLength { get => rightLength; }
    public int LeftLength { get => leftLength; }
}
