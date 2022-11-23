using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : GameEntity, IDamageable, IPushable, IObstruct, ICanDie
{
    Location location;
    public event Action<GameEntity> HasDied;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.ENEMY; }

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] DamageableComponent damageable;
    [SerializeField] PushableComponent pushable;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] Obstruction obstruction;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] EnemyAction enemyAction;

    public IDamageable Damageable { get => damageable; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public IPushable Pushable { get => pushable; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }    
    public EnemyMovement EnemyMovement { get => enemyMovement; }
    public EnemyAction EnemyAction { get => enemyAction; }
    public IObstruct Obstruct { get => obstruction; }
    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
