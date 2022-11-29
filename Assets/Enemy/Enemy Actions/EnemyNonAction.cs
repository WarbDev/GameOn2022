using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyNonAction : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;

    public override void DoEnemyAction()
    {
        ActionFinished?.Invoke(this);
    }
}
