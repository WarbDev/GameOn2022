using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyBowAction : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    [SerializeField] protected float damage;
    [SerializeField] protected int range;

    public int Range { get => range; }

    public override void DoEnemyAction()
    {
        LocationUtility.TryGetPlayer((0, GameEntity.Location.Y), out Player player);
        IDamageable damageable = player;
        DamageLog log = damageable.DealDamage(new Damage(damage, GameEntity));
        var animation = Animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENEMY_RANGED, new(log));

        animation.AnimationFinished += animationFinished;
        void animationFinished(EntityAnimation<HurtAnimationProperties> animation)
        {
            animation.AnimationFinished -= animationFinished;
            ActionFinished?.Invoke(this);
        }
    }
}