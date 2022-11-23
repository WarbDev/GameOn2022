using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DamageableComponent : EntityComponent, IDamageable
{
    public IDamageable Damageable { get => this; }
    public GameEntity Entity { get => GameEntity; }
    public abstract DamageLog DealDamage(DamageDetails damage);

    
}

public interface IDamageable : IHaveGameEntity
{
    public IDamageable Damageable { get; }

    public DamageLog DealDamage(DamageDetails damage)
    {
        return Damageable.DealDamage(damage);
    }
}