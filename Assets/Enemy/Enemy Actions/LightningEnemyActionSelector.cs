using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyMeleeAction))]
[RequireComponent(typeof(EnemyNonAction))]
[RequireComponent(typeof(EnemyChargeLightning))]
[RequireComponent(typeof(EnemyLightningAttack))]
public class LightningEnemyActionSelector : EnemyActionSelectionComponent
{
    [SerializeField] EnemyMeleeAction meleeAction;
    [SerializeField] EnemyNonAction nonAction;
    [SerializeField] EnemyLightningAttack lightningAction;
    [SerializeField] EnemyChargeLightning chargeLightningAction;


    public override event Action<EnemyAction> ActionFinished;

    public override void MakeAction()
    {
        meleeAction.ActionFinished += onActionFinished;
        nonAction.ActionFinished += onActionFinished;
        lightningAction.ActionFinished += onActionFinished;
        chargeLightningAction.ActionFinished += onActionFinished;

        if (Stun.StunDuration > 0)
        {
            stunnedAction.DoEnemyAction();
            return;
        }

        if (EnemyAction.IsPlayerLineInHorizontalRange(1, lightningAction.Range, GameEntity.Location) && LocationUtility.HasPlayer((0, GameEntity.Location.Y)))
        {
            if (chargeLightningAction.TimesCharged >= chargeLightningAction.ChargeRequired)
            {
                lightningAction.DoEnemyAction();
                return;
            }
        }

        if (EnemyAction.NonAlliedObstructionInFront(GameEntity.Location, ObstructionChecker) != null)
        {
            meleeAction.DoEnemyAction();
            return;
        }

        if (chargeLightningAction.TimesCharged < chargeLightningAction.ChargeRequired)
        {
            chargeLightningAction.DoEnemyAction();
            return;
        }


        nonAction.DoEnemyAction();
        return;

        void onActionFinished(EnemyAction a)
        {
            meleeAction.ActionFinished -= onActionFinished;
            nonAction.ActionFinished -= onActionFinished;
            lightningAction.ActionFinished -= onActionFinished;
            chargeLightningAction.ActionFinished -= onActionFinished;
            ActionFinished?.Invoke(a);
        }
    }
}
