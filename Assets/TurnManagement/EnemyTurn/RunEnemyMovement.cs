using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Runs each enemy's movement. Has FinishedRunningMovements as a callback.
/// </summary> 
public class RunEnemyMovement : MonoBehaviour
{
    [Tooltip("If checked, the script will run the next enemy turn after the timer value. " +
        "If unchecked, the script will run the next enemy turn after each animation ends.")]
    [SerializeField] bool useTimer = true;

    [Tooltip("The time in seconds that must pass until the next enemy movement is ran if UseTimer is checked.")]
    [SerializeField] float timer = 0.1f;

    Coroutine enemyTurnMovementCoroutine;

    // Called once all enemies are finished with their movement.
    public event Action Finished;

    /// <summary>
    /// Gets all of the enemies on the map and runs through each of their turns. Invokes Finishedonce done.
    /// </summary> 
    public void RunEnemyMovements()
    {
        enemyTurnMovementCoroutine = StartCoroutine(RunEachMovement());
    }

    IEnumerator RunEachMovement()
    {
        yield return null;
        bool readyForNext;
        Queue<Enemy> enemiesToCalculate = LocationUtility.MakeQueueOfEnemiesInColumns();

        // This loop stops after the last enemy's turn has been ran.
        while (enemiesToCalculate.Count > 0)
        {
            // Reset to false to not execute all animations at once.
            readyForNext = false;
            var enemy = enemiesToCalculate.Dequeue();

            // If not using the timer, pause the method until the animation of the latest enemy turn ends.
            if (!useTimer)
            {
                enemy.EnemyMovement.MovementFinished += IndicateReady;
                enemy.EnemyMovement.DoTurnMovement();
                yield return new WaitUntil(() => readyForNext);
            }

            // If using the timer, pause the method until the timer in seconds has passed.
            if (useTimer)
            {
                enemy.EnemyMovement.DoTurnMovement();
                yield return new WaitForSeconds(timer);
            }
        }

        Finished?.Invoke();

        // Simple unsubscription and bool set to allow the Coroutine to continue.
        void IndicateReady(EnemyMovement enemyMovement)
        {
            enemyMovement.MovementFinished -= IndicateReady;
            readyForNext = true;
        }
    }
}