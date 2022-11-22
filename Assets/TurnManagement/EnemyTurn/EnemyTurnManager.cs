using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyTurnManager : MonoBehaviour
{

    [SerializeField] RunEnemyMovement enemyMovementRunner;
    [SerializeField] EnemyActionRunner enemyActionRunner;
    public event Action EnemyRoundFinished;

    private void Start()
    {
        enemyMovementRunner.Finished += enemyActionRunner.RunEnemyActions;
        enemyActionRunner.Finished += () => EnemyRoundFinished?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            RunEnemyTurn();
        }
    }

    public void RunEnemyTurn()
    {
        enemyMovementRunner.RunEnemyMovements();
    }


}
