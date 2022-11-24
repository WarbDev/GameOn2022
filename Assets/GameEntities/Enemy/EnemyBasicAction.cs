using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBasicAction : EnemyAction
{
    [SerializeField] float damage;
    public override event Action<EnemyAction> ActionFinished;


    public override void DoEnemyAction()
    {
        IDamageable entity = HostileEntityInFront();
        if (entity != null)
        {
            entity.DealDamage(new Damage(damage, GameEntity));
        }
    }

    IDamageable HostileEntityInFront()
    {
        var frontLocation = LocationUtility.NextClosestToCenter(GameEntity.Location);
        IDamageable damageable = null;

        if (!LocationUtility.HasEnemy(frontLocation) && LocationUtility.HasObstructionsAtPosition(frontLocation))
        {
            damageable = LocationUtility.GetDamageableInLocation(frontLocation);
        }

        return damageable;
    }
}
