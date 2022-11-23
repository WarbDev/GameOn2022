using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageableHealth : Damageable
{
    //Inherits an event from Damageable called HasDied

    [SerializeField] Health health;
    public override DamageLog DealDamage(DamageDetails damage)
    {
        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);
        return new DamageLog(this, oldHealth, newHealth, damage);
    }
}
