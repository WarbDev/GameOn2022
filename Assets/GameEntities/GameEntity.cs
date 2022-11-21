using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour, IHaveGameEntity
{
    public GameEntity Entity { get => this; }
    public abstract Location Location { get; protected set; }
    public abstract EntityType EntityType { get; }

    public abstract void DestroyEntity();
    public void SetLocation(Location newLocation)
    {
        Location = newLocation;
    }
}

public interface IHaveGameEntity
{
    public GameEntity Entity { get; }
}


public enum EntityType
{
    PLAYER,
    ENEMY,
    MAPTILE,
    TERRAIN
}