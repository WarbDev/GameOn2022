using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBatch : BatchBase, IEnumerator<ENEMY_TYPE>
{
    int curIndex;
    List<ENEMY_TYPE> unusedPool;

    object IEnumerator.Current => currentEnemy;

    void Awake()
    {
        unusedPool = new List<ENEMY_TYPE>();
        unusedPool.AddRange(batchPool);
        curIndex = -1;
    }

    public override bool MoveNext()
    {
        if (++curIndex >= batchPool.Count)
        {
            return false;
        }
        else
        {
            int randomIndex = Random.Range(0, unusedPool.Count);
            currentEnemy = unusedPool[randomIndex];
            unusedPool.Remove(currentEnemy);
            return true;
        }
    }

    public override void Reset() { curIndex = -1; }

    public override IEnumerator<ENEMY_TYPE> GetEnumerator()
    {
        return this;
    }
}
