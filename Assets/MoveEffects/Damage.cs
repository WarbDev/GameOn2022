using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DamageEffect
{
    public static event Action<DamageLog> AppliedDamage;
    public static DamageLog Apply(DamageableComponent target, DamageDetails damage)
    {
        DamageLog damageLog = target.DealDamage(damage);
        AppliedDamage?.Invoke(damageLog);
        return damageLog;
    }
}

public class DamageLog
{
    DamageDetails damage;
    IDamageable target;
    float oldHealth;
    float newHealth;

    public DamageDetails Damage { get => damage; }
    public IDamageable Target { get => target; }
    public float OldHealth { get => oldHealth; }
    public float NewHealth { get => newHealth; }
    

    public DamageLog(IDamageable victim, float oldHP, float newHP, DamageDetails dmg)
    {
        target = victim;
        oldHealth = oldHP;
        newHealth = newHP;
        damage = dmg;
    }
}

public class DamageDetails
{
    public float Base;
    public GameEntity DamageSource;
    public DamageDetails(float Base, GameEntity DamageSource)
    {
        this.Base = Base;
        this.DamageSource = DamageSource;
    }

    public DamageDetails() { }
}

public interface IHaveHealth
{
    Health Health { get; }
}