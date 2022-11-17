using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEntity
{
    public GameObject GameObject { get; }
    public Location Location { get; set; }
    public EntityType EntityType { get; }

    public bool IsObstructedBy(IGameEntity entity);

    public void DestroyEntity();
}

public enum EntityType
{
    PLAYER,
    ENEMY,
    MAPTILE
}