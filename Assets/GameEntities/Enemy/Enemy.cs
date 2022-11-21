using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameEntity, IHaveDamageable, IPushableEntity, IObstructingEntity
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.ENEMY; }
    
    [SerializeField] Damageable damageable;
    public Damageable Damageable { get => damageable; }

    [SerializeField] SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

    [SerializeField] Pushable pushable;
    public Pushable Pushing { get => pushable; }
    public ObstructionChecker ObstructionChecker { get => pushable.ObstructionChecker; }
    [SerializeField] Obstruction obstruction;
    [SerializeField] EnemyMovement enemyMovement;
    public EnemyMovement EnemyMovement { get => enemyMovement; }
    public IObstruct Obstruct { get => obstruction; }

    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
