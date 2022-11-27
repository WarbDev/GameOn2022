using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTerrain : TerrainBase
{
    Location location;

    public GameObject GameObject { get => gameObject; }

    public override Location Location { get => location; protected set => location = value; }

    public override EntityType EntityType { get => EntityType.TERRAIN; }
    

    public int TurnsUntilExtinguish;

    int damage = 1;

    private void Awake()
    {

        //Subscribe to roundEnd event;
    }

    public override void DestroyEntity()
    {
        //Unsubscribe to roundEnd event;
        Destroy(gameObject);
    }

    private void onRoundEnd()
    {
        TurnsUntilExtinguish--;
        if(TurnsUntilExtinguish <= 0)
        {
            DestroyEntity();
        }
    }

    public override void OnEntityMoveOver(GameEntity entity)
    {
        IDamageable damageable = entity as IDamageable;
        if (damageable != null)
        {
            DamageLog log = damageable.DealDamage(new Damage(damage, this));
            IAnimatable animatable = entity as IAnimatable;
            if (animatable != null)
            {
                animatable.PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(log));
            }
        }
    }
}
