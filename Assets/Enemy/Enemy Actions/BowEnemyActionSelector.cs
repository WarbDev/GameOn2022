using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EnemyMeleeAction))]
[RequireComponent(typeof(EnemyNonAction))]
[RequireComponent(typeof(EnemyBowAction))]
public class BowEnemyActionSelector : EnemyActionSelectionComponent
{
    [SerializeField] EnemyMeleeAction meleeAction;
    [SerializeField] EnemyNonAction nonAction;
    [SerializeField] EnemyBowAction bowAction;
    public override event Action<EnemyAction> ActionFinished;

    public override void MakeAction()
    {
        meleeAction.ActionFinished += onActionFinished;
        nonAction.ActionFinished += onActionFinished;
        bowAction.ActionFinished += onActionFinished;

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

        if (EnemyAction.IsPlayerLineInHorizontalRange(1, bowAction.Range, GameEntity.Location))
        {
            Location location = GameEntity.Location;
            int absoluteX = Math.Abs(location.X);
            int centerDirection = LocationUtility.DirectionTowardsCenter(location);
            var locationsInRange = EnemyAction.HorizontalLocationsInRange(1, absoluteX, centerDirection, location);
            if (locationsInRange.Contains((0, location.Y)))
            {
                locationsInRange.Remove((0, location.Y));
            }

            if (!ObstructionChecker.IsObstructedBy(locationsInRange))
            {
                bowAction.DoEnemyAction();
                return;
            }
        }


        nonAction.DoEnemyAction();
        return;

        void onActionFinished(EnemyAction a)
        {
            meleeAction.ActionFinished -= onActionFinished;
            nonAction.ActionFinished -= onActionFinished;
            bowAction.ActionFinished -= onActionFinished;
            ActionFinished?.Invoke(a);
        }
    }
}
