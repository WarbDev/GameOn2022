using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyActionRunner : MonoBehaviour
{
    Coroutine enemyTurnActionCoroutine;
    public event Action Finished;
    public event Action ActionTick;

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
            enemy.EnemyAction.MakeAction();
            yield return new WaitUntil(() => readyForNext == true);
            ActionTick?.Invoke();
        }
        yield return new WaitForSeconds(0.5f);
        Finished?.Invoke();

        // Simple unsubscription and bool set to allow the Coroutine to continue.
        void IndicateReady(EnemyAction enemyAction)
        {
            enemyAction.ActionFinished -= IndicateReady;
            readyForNext = true;
        }
    }
}
