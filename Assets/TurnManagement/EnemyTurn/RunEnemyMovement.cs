using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RunEnemyMovement : MonoBehaviour
{
    MoveLog CalculateEnemyMovement(Enemy enemy)
    {
        MoveLog log = enemy.DoTurnMovement();
        return log;
    }
}

public class EnemyMovementTurn
{
    public event Action ReadyForNextTurn;
}