using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPhase : MonoBehaviour
{
    [SerializeField] RunEnemyMovement enemyMovementRunner;
    [SerializeField] EnemyActionRunner enemyActionRunner;
    [SerializeField] WaveRunner waveRunner;

    public void StartRound(Action OnEnd)
    {
        StartCoroutine(PhaseRoutine(OnEnd));
    }

    IEnumerator PhaseRoutine(Action OnEnd)
    {
        // Run wave
        bool isFinished = false;
        waveRunner.Finished += SetFinished;
        waveRunner.RunNextBatch();
        while (!isFinished)
        {
            yield return null;
        }
        isFinished = false;
        waveRunner.Finished -= SetFinished;

        // Run movements
        enemyMovementRunner.Finished += SetFinished;
        enemyMovementRunner.RunEnemyMovements();
        while (!isFinished)
        {
            yield return null;
        }
        isFinished = false;
        enemyMovementRunner.Finished -= SetFinished;

        // Run actions
        enemyActionRunner.Finished += SetFinished;
        enemyActionRunner.RunEnemyActions();
        while (!isFinished)
        {
            yield return null;
        }
        isFinished = false;
        enemyActionRunner.Finished -= SetFinished;

        

        // Finish
        OnEnd();

        void SetFinished() => isFinished = true;
    }
}
