using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyRound : MonoBehaviour
{
    [SerializeField] RunEnemyMovement enemyMovementRunner;
    [SerializeField] EnemyActionRunner enemyActionRunner;

    public event Action End;
    public event Action Begin;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovementRunner.Finished += OnMovementRunnerFinish;
        enemyActionRunner.Finished += OnActionRunnerFinish;
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
        End?.Invoke();
    }
}
