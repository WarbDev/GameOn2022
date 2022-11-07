using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EntityCollection
{
    public static EntityCollection<Enemy> EnemyCollection = new();
    public static EntityCollection<Player> PlayerCollection = new();
    public static EntityCollection<MapTile> MapTileCollection = new();
}

public class EntityCollection<T> where T : IGameEntity
{
    HashSet<T> entities = new();

    public event Action<T> EntityAdded;
    public event Action<T> EntityRemoved;

    public void AddEntity(T entity)
    {
        entities.Add(entity);
        EntityAdded?.Invoke(entity);
    }

    public void RemoveEntity(T entity)
    {
        entities.Remove(entity);
        EntityRemoved?.Invoke(entity);
    }
}
