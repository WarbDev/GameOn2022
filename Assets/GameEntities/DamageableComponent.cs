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
    public IDamageable Damageable { get; } //Suggested: Use DamageableComponent, attached to the Prefab that IDamageable is attached to


    //Don't implement DealDamage in the main class that is attached to a prefab.
    //Implement it through the Damageable Property.
    public DamageLog DealDamage(DamageDetails damage)
    {
        return Damageable.DealDamage(damage);
    }
}