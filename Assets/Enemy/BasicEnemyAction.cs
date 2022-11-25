using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAction : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    static int timesDone = 0;

    public override void DoEnemyAction()
    {
        timesDone++;
        Debug.Log("Enemy did action! " + timesDone);
        ActionFinished?.Invoke(this);
    }
}
