using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNextEnemyTurnToHandle : MonoBehaviour, INextEnemyTurnSelector
{
    bool initialized = false;
    Queue<Enemy> toCalculate;

    Queue<Enemy> Initialize()
    {
        Queue<Enemy> enemiesToCalculate = new();
        foreach (var column in LocationUtility.AllColumns())
        {
            foreach (var location in column)
            {
                if (LocationUtility.TryGetEnemy(location, out Enemy enemy))
                {
                    enemiesToCalculate.Enqueue(enemy);
                }
            }
        }
        initialized = true;
        return enemiesToCalculate;
    }

    public Enemy NextEnemy()
    {
        if (!initialized)
        {
            toCalculate = Initialize();
        }

        if (toCalculate.TryDequeue(out Enemy enemy))
        {
            return enemy;
        }
        else return null;

    }
}

public interface INextEnemyTurnSelector
{
    public Enemy NextEnemy();
}