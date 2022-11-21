using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RunEnemyMovement : MonoBehaviour
{
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
        return enemiesToCalculate;
    }
}