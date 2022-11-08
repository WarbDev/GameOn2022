using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEntity
{
    public Location Location { get; set; }
    public EntityType EntityType { get; }
}

public enum EntityType
{
    PLAYER,
    ENEMY,
    MAPTILE
}