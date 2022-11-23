using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DamageEffect
{
    public static event Action<DamageLog> AppliedDamage;
    public static DamageLog Apply(DamageableComponent target, Damage damage)
    {
        DamageLog damageLog = target.DealDamage(damage);
        AppliedDamage?.Invoke(damageLog);
        return damageLog;
    }
}

public class DamageLog
{
    Damage damage;
    IDamageable target;
    float oldHealth;
    float newHealth;

    public Damage Damage { get => damage; }
    public IDamageable Target { get => target; }
    public float OldHealth { get => oldHealth; }
    public float NewHealth { get => newHealth; }
    

    public DamageLog(IDamageable victim, float oldHP, float newHP, Damage dmg)
    {
        target = victim;
        oldHealth = oldHP;
        newHealth = newHP;
        damage = dmg;
    }
}

public class Damage
{
    public float Base;
    public GameEntity DamageSource;
    public Damage(float Base, GameEntity DamageSource)
    {
        this.Base = Base;
        this.DamageSource = DamageSource;
    }

    public Damage() { }
}

public interface IHaveHealth
{
    Health Health { get; }
}