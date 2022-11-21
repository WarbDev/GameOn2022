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
    public IObstruct Obstruct { get => obstruction; }

    public MoveLog DoTurnMovement()
    {
        if (LocationUtility.TryGetEnemy((Location.X + DirectionTowardsPlayers(), Location.Y), out Enemy enemy))
        {
            return null;
        }
        Location newLocation = (Location.X + DirectionTowardsPlayers(), Location.Y);
        MoveLog moveLog = new MoveLog(this, location, newLocation);
        GameMap.MoveEnemy(this, newLocation);
        return moveLog;

        int DirectionTowardsPlayers()
        {
            return System.Math.Sign(Location.X * -1);
        }
    }

    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
