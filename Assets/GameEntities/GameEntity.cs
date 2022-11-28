using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour, IHaveGameEntity
{
    public GameEntity Entity { get => this; }
    public abstract Location Location { get; protected set; }
    public abstract EntityType EntityType { get; }

    public virtual void RemoveFromEntityTracker()
    {
        if (Entities.HasEntity(Entity))
        {
            Entities.RemoveEntity(Entity);
        }
    }

    public void OnDestroy()
    {
        RemoveFromEntityTracker();
    }

    public virtual void SetLocation(Location newLocation)
    {
        Location = newLocation;

        if (LocationUtility.TryGetTile(newLocation, out MapTile mapTile))
        {
            transform.SetParent(mapTile.transform);
        }
    }

    /// <summary>
    /// Set isInitializing to true if used for the purpose of initializing a new GameEntity.
    /// </summary>
    public virtual void SetLocation(Location newLocation, bool isInitializing)
    {
        SetLocation(newLocation);

        if (isInitializing)
            transform.localPosition = Vector3.zero;
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