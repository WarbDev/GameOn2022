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
        IDamageable damageable = HostileEntityInFront();
        if (damageable != null)
        {
            var animatable = damageable.Entity as IAnimatable;
            var log = damageable.DealDamage(new Damage(damage, GameEntity));

            if (animatable != null)
            {
                var animation = animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENTITY_HURT, new(log));
                animation.AnimationFinished += animationFinished;
            }

            void animationFinished(EntityAnimation<HurtAnimationProperties> animation)
            {
                animation.AnimationFinished -= animationFinished;
                ActionFinished?.Invoke(this);
            }
        }
        else
        {
            ActionFinished?.Invoke(this);
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
