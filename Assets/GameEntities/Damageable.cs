using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : EntityComponent
{
    public abstract DamageLog DealDamage(DamageDetails damage);
}

public interface IHaveDamageable
{
    public Damageable Damageable { get; }
}