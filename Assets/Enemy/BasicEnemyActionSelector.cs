using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMeleeAction))]
[RequireComponent(typeof(EnemyNonAction))]
public class BasicEnemyActionSelector : EnemyActionSelectionComponent
{
    [SerializeField] EnemyMeleeAction meleeAction;
    [SerializeField] EnemyNonAction nonAction;
    public override event Action<EnemyAction> ActionFinished;

    public override void MakeAction()
    {
        meleeAction.ActionFinished += onActionFinished;
        nonAction.ActionFinished += onActionFinished;

        if (Stun.StunDuration > 0)
        {
            nonAction.DoEnemyAction();
            return;
        }

        if (EnemyAction.NonAlliedObstructionInFront(GameEntity.Location, ObstructionChecker) != null)
        {
            meleeAction.DoEnemyAction();
            return;
        }

        nonAction.DoEnemyAction();
        return;

        void onActionFinished(EnemyAction a)
        {
            meleeAction.ActionFinished -= onActionFinished;
            nonAction.ActionFinished -= onActionFinished;
            ActionFinished?.Invoke(a);
        }
    }
}

