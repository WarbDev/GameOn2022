using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyLightningAttack : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    [SerializeField] protected float damage;
    [SerializeField] protected int range;
    [SerializeField] EnemyChargeLightning chargeLightning;

    public int Range { get => range; }



    public override void DoEnemyAction()
    {
        var locations = EnemyAction.HorizontalLocationsInRange(1, Range, LocationUtility.DirectionTowardsCenter(GameEntity.Location), GameEntity.Location);
        List<IDamageable> damageables = new();
        List<DamageLog> damageLogs = new();
        foreach (var location in locations)
        {
            var damageable = LocationUtility.GetDamageableInLocation(location);
            if (damageable != null)
            {
                damageables.Add(damageable);
            }
        }

        foreach (var damageable in damageables)
        {
            damageLogs.Add(damageable.DealDamage(new Damage(damage, GameEntity)));
        }

        var animation = Animatable.PlayAnimation<LightningStrikeProperties>(ANIMATION_ID.ENEMY_RANGED, new(damageLogs));

        animation.AnimationFinished += animationFinished;
        void animationFinished(EntityAnimation<LightningStrikeProperties> animation)
        {
            chargeLightning.ResetCharges();
            animation.AnimationFinished -= animationFinished;
            ActionFinished?.Invoke(this);
        }
    }
}