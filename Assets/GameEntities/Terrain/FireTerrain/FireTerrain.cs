using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireTerrain : TerrainBase
{
    Location location;

    public GameObject GameObject { get => gameObject; }

    public override Location Location { get => location; protected set => location = value; }

    public override EntityType EntityType { get => EntityType.TERRAIN; }
    

    public int TurnsUntilExtinguish;

    int damage = 1;

    public override void OnRoundTick()
    {
        TurnsUntilExtinguish--;
        if(TurnsUntilExtinguish <= 0)
        {
            RemoveFromEntityTracker();
            Destroy(gameObject);
        }
    }

    public override Func<bool> OnEntityMoveOver(GameEntity entity)
    {
        bool isFinished = false;
        Func<bool> isFinishedFunc = ()=>isFinished;
        IDamageable damageable = entity as IDamageable;
        if (damageable != null)
        {
            DamageLog log = damageable.DealDamage(new Damage(damage, this));
            IAnimatable animatable = entity as IAnimatable;
            if (animatable != null)
            {
                var animation = animatable.PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(log));
                animation.AnimationFinished += OnAniFin;

                void OnAniFin(EntityAnimation<HurtAnimationProperties> a)
                {
                    a.AnimationFinished -= OnAniFin;
                    isFinished = true;
                }
            }
            else
            {
                isFinished = true;
            }
        }
        else isFinished = true;
        return isFinishedFunc;
    }
}
