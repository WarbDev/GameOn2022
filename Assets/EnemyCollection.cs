using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
 * This is the single source of truth regarding the
 * existance of enemies.
 * 
 * Any class that adds or removes an enemy into the game must notify this class,
 * and it will notify any class that is listening to these things.
 */
public static class EnemyCollection
{
    private static HashSet<Enemy> Enemies = new();

    public static event Action<Enemy> EnemyAdded;
    public static event Action<Enemy> EnemyRemoved;

    public static void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
        EnemyAdded?.Invoke(enemy);
    }

    public static void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        EnemyRemoved?.Invoke(enemy);
    }
}
