using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity, IHaveHealth, ICanDie, IDamageable, IAnimatable, IObstruct
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.PLAYER; }

    [SerializeField] Health health;
    public Health Health { get => health; }

    [SerializeField] DamageableWithHealthComponent damageableHealth;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] Obstruction obstruction;
    [SerializeField] AnimatableEntity playerAnimations;

    public IDamageable Damageable { get => damageableHealth; }
    public IObstruct Obstruct { get => obstruction; }
    public IAnimatable Animatable { get => playerAnimations; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }


    public string Name;

    
    public event Action<GameEntity> HasDied;

    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
