using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageableWithHealthComponent : DamageableComponent, IDamageable
{
    [SerializeField] Health health;

    public override DamageLog DealDamage(Damage damage)
    {
        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);
        return new DamageLog(this, oldHealth, newHealth, damage);
    }
}
