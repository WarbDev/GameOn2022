using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DamageableComponent : EntityComponent, IDamageable, ICanDie
{
    public abstract bool IsDead { get; }
    public IDamageable Damageable { get => this; }
    public GameEntity Entity { get => GameEntity; }
    public abstract DamageLog DealDamage(Damage damage);
    public abstract void Kill();
}

public interface IDamageable : IHaveGameEntity
{
    public bool IsDead { get; }
    public IDamageable Damageable { get; } //Suggested: Use DamageableComponent, attached to the Prefab that IDamageable is attached to


    //Don't implement DealDamage in the main class that is attached to a prefab.
    //Implement it through the Damageable Property.
    public DamageLog DealDamage(Damage damage)
    {
        return Damageable.DealDamage(damage);
    }
}