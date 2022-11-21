using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DamageEffect
{
    public static event Action<DamageLog> AppliedDamage;
    public static DamageLog Apply(Damageable target, DamageDetails damage)
    {
        DamageLog damageLog = target.DealDamage(damage);
        AppliedDamage?.Invoke(damageLog);
        return damageLog;
    }
}

public class DamageLog
{
    DamageDetails damage;
    Damageable target;
    float oldHealth;
    float newHealth;

    public DamageDetails Damage { get => damage; }
    public Damageable Target { get => target; }
    public float OldHealth { get => oldHealth; }
    public float NewHealth { get => newHealth; }
    

    public DamageLog(Damageable victim, float oldHP, float newHP, DamageDetails dmg)
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