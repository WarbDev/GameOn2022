using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity, IHaveHealth, ICanDie, IDamageable
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.PLAYER; }

    [SerializeField] Health health;
    public Health Health { get => health; }

    [SerializeField] DamageableWithHealthComponent damageableHealth;
    public IDamageable Damageable { get => damageableHealth; }

    public string Name;

    
    public event Action<GameEntity> HasDied;

    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
