using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RunEnemyMovement : MonoBehaviour
{
    [Tooltip("If checked, the script will run the next enemy turn after the timer value. " +
        "If unchecked, the script will run the next enemy turn after each animation ends.")]
    [SerializeField] bool useTimer = true;

    [Tooltip("The time in seconds that must pass until the next enemy movement is ran if UseTimer is checked.")]
    [SerializeField] float timer = 0.1f;

    Coroutine enemyTurnMovementCoroutine;

    // Called once all enemies are finished with their movement.
    public event Action FinishedRunningMovements;

    public void RunEnemyMovements()
    {
        enemyTurnMovementCoroutine = StartCoroutine(RunEachMovement());
    }

    // Get all enemies. Goes through each column from the center to the right,
    // then goes through each column from center to the left
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

    IEnumerator RunEachMovement()
    {
        bool readyForNext;
        Queue<Enemy> enemiesToCalculate = Initialize();

        // This loop stops after the last enemy's turn has been ran.
        while (enemiesToCalculate.Count > 0)
        {
            // Reset to false to not execute all animations at once.
            readyForNext = false;
            var enemy = enemiesToCalculate.Dequeue();

            // If not using the timer, pause the method until the animation of the latest enemy turn ends.
            if (!useTimer)
            {
                enemy.EnemyMovement.EnemyMovementAnimationFinished += IndicateReady;
                yield return new WaitUntil(() => readyForNext);
            }

            // If using the timer, pause the method until the timer in seconds has passed.
            else
            {
                yield return new WaitForSeconds(timer);
            }
            
            enemy.EnemyMovement.DoTurnMovement();
            
        }

        StopCoroutine(enemyTurnMovementCoroutine);
        FinishedRunningMovements?.Invoke();

        // Simple unsubscription and bool set to allow the Coroutine to continue.
        void IndicateReady(EnemyMovement enemyMovement)
        {
            enemyMovement.EnemyMovementAnimationFinished -= IndicateReady;
            readyForNext = true;
        }
    }
}