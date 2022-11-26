using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPhase : MonoBehaviour
{
    [SerializeField] RunEnemyMovement enemyMovementRunner;
    [SerializeField] EnemyActionRunner enemyActionRunner;
    [SerializeField] PlayerPhase playerPhase;

    public event Action Finished;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovementRunner.Finished += OnMovementRunnerFinish;
        enemyActionRunner.Finished += OnActionRunnerFinish;
        playerPhase.Finished += StartRound;
    }

    public void StartRound()
    {
        enemyMovementRunner.RunEnemyMovements();
    }

    void OnMovementRunnerFinish()
    {
        enemyActionRunner.RunEnemyActions();
    }

    void OnActionRunnerFinish()
    {
        Finished?.Invoke();
    }
}
