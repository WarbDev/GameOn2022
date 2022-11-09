using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatchBase : MonoBehaviour, IEnumerable<ENEMY_TYPE>
{
    [SerializeField] protected List<ENEMY_TYPE> batchPool = new();

    protected ENEMY_TYPE currentEnemy;
    public ENEMY_TYPE Current { get => currentEnemy; }

    public abstract IEnumerator<ENEMY_TYPE> GetEnumerator();

    public abstract bool MoveNext();

    public abstract void Reset();

    public void Dispose() { }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

