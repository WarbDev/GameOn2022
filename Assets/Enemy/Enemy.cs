using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : GameEntity, IDamageable, IPushable, IObstruct, IAnimatable, IStunnable
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.ENEMY; }
    public bool IsDead { get => Damageable.IsDead; }

    [SerializeField] int stunDuration = 0;
    public int StunDuration { get => stunDuration; }


    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] DamageableComponent damageable;
    [SerializeField] PushableComponent pushable;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] Obstruction obstruction;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyActionSelectionComponent enemyActionManager;
    [SerializeField] AnimatableEntity enemyAnimations;
    [SerializeField] GameObject StunBorder;

    public IDamageable Damageable { get => damageable; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public IPushable Pushable { get => pushable; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }    
    public EnemyMovement EnemyMovement { get => enemyMovement; }
    public EnemyActionSelectionComponent EnemyAction { get => enemyActionManager; }
    public IObstruct Obstruct { get => obstruction; }
    public IAnimatable Animatable { get => enemyAnimations; }

    public virtual void AddStun(int duration)
    {
        stunDuration += duration;
        StunBorder.SetActive(true);
    }

    public void OnRoundTick()
    {
        if (stunDuration > 0)
        {
            stunDuration -= 1;
        }
        if (stunDuration == 0)
        {
            StunBorder.SetActive(false);
        }
    }
}
