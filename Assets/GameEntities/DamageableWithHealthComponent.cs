using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageableWithHealthComponent : DamageableComponent, IDamageable
{
    [SerializeField] Health health;

    [SerializeField] DeathComponent deathManager;
    public override bool IsDead { get => health.CurrentHealth <= 0; }

    public override DamageLog DealDamage(Damage damage)
    {
        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);

        if (IsDead)
        {
            Kill();
        }
        return new DamageLog(this, oldHealth, newHealth, damage);
    }

    public override void Kill()
    {
        deathManager.Die();
    }
}
