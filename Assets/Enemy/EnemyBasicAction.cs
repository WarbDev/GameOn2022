using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// obsolete
public class EnemyBasicAction : EnemyAction, IObstructionChecker
{
    public override event Action<EnemyAction> ActionFinished;


    public override void DoEnemyAction()
    {
        //IDamageable damageable = NonAlliedObstructionInFront(GameEntity.Location, ObstructionChecker);
        //IAnimatable animatable = damageable.Entity as IAnimatable;
        //DamageLog log = damageable.DealDamage(new Damage(0f, GameEntity));

        //var animation = animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENTITY_HURT, new(log));

        //animation.AnimationFinished += animationFinished;
        //void animationFinished(EntityAnimation<HurtAnimationProperties> animation)
        //{
        //    animation.AnimationFinished -= animationFinished;
        //    ActionFinished?.Invoke(this);
        //}
    }
}
