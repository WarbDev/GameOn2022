using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBasicAction : EnemyAction
{
    [SerializeField] AnimatableEntity animatableEntity;
    public override event Action<EnemyAction> ActionFinished;

    public override void DoEnemyAction()
    {

    }

    IDamageable HostileEntityInFront()
    {
        var frontLocation = LocationUtility.NextClosestToCenter(GameEntity.Location);
        
        if (!LocationUtility.HasEnemy(frontLocation) && LocationUtility.HasObstructionsAtPosition(frontLocation))
        {
            var damageables = LocationUtility.GetDamageablesInLocation(frontLocation);
            if (damageables.Count > 0)
            {
                return null;
            }
        }

        return null;
    }
}
