using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyActionRunner : MonoBehaviour
{
    Coroutine enemyTurnActionCoroutine;
    public event Action Finished;

    public void RunEnemyActions()
    {
        enemyTurnActionCoroutine = StartCoroutine(RunEachAction());
    }

    IEnumerator RunEachAction()
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

            enemy.EnemyAction.ActionFinished += IndicateReady;
            enemy.EnemyAction.DoEnemyAction();
            yield return new WaitUntil(() => readyForNext == true);
        }

        Finished?.Invoke();

        // Simple unsubscription and bool set to allow the Coroutine to continue.
        void IndicateReady(EnemyAction enemyAction)
        {
            enemyAction.ActionFinished -= IndicateReady;
            readyForNext = true;
        }
    }
}