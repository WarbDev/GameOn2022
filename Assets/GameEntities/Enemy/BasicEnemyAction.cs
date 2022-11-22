using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAction : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;

    public override void DoEnemyAction()
    {
        Debug.Log("Enemy did action!");
        ActionFinished?.Invoke(this);
    }
}
