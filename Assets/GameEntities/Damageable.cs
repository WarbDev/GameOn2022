using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Damageable : EntityComponent
{
    public abstract DamageLog DealDamage(DamageDetails damage);

    public Action<GameEntity> HasDied;
}

public interface IHaveDamageable
{
    public Damageable Damageable { get; }
}