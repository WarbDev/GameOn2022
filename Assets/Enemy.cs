using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGameEntity, ICanMakeMoveRequests
{
    Location location;

    public Location Location { get => location; set => location = value;}
    public EntityType EntityType { get => EntityType.ENEMY; }

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
    }

    int DirectionTowardsPlayers()
    {
        return System.Math.Sign(Location.X * -1);
    }
}
