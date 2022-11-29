using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyMeleeAction : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    [SerializeField] float damage;

    public override void DoEnemyAction()
    {
        IDamageable damageable = NonAlliedObstructionInFront(GameEntity.Location, ObstructionChecker);
        DamageLog log = damageable.DealDamage(new Damage(damage, GameEntity));
        var animation = Animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENEMY_MELEE, new(log));

        animation.AnimationFinished += animationFinished;
        void animationFinished(EntityAnimation<HurtAnimationProperties> animation)
        {
            animation.AnimationFinished -= animationFinished;
            ActionFinished?.Invoke(this);
        }
    }
}