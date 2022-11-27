using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using EditorGUITable;

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

    public (int, int, int) WaveMapSize { get => (leftLength, rightLength, height); }

    [Tooltip("Represents a set of batches. Each batch will not run until the first one is finished.")]
    [Table (new string[] {"Batch: Sortable(false)", "Delay: Sortable(false)", "Notes: Sortable(false)"})] [SerializeField] List<WaveBatchEntry> batchEntries = new();
    public List<BatchBase> Batches { get => batchEntries.Select((batch) => batch.Batch).ToList(); }
    public List<int> Delays { get => batchEntries.Select((batch) => batch.Delay).ToList(); }
}

[System.Serializable]
public class WaveBatchEntry
{
    public BatchBase Batch;
    public int Delay;
    public string Notes;
}