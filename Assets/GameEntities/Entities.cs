using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Entities : MonoBehaviour
{
    public static Entities<Enemy> EnemyCollection = new();
    public static Entities<Player> PlayerCollection = new();
    public static Entities<MapTile> MapTileCollection = new();
    public static Entities<TerrainBase> TerrainCollection = new();

    public static void RemoveEntity(GameEntity entity)
    {
        if (entity.EntityType == EntityType.ENEMY)
        {
            EnemyCollection.RemoveEntity(entity as Enemy);
        }
        if (entity.EntityType == EntityType.MAPTILE)
        {
            MapTileCollection.RemoveEntity(entity as MapTile);
        }
        if (entity.EntityType == EntityType.PLAYER)
        {
            PlayerCollection.RemoveEntity(entity as Player);
        }
        if (entity.EntityType == EntityType.TERRAIN)
        {
            TerrainCollection.RemoveEntity(entity as TerrainBase);
        }
    }

    public static void AddEntity(GameEntity entity)
    {
        if (entity.EntityType == EntityType.ENEMY)
        {
            EnemyCollection.AddEntity(entity as Enemy);
        }
        if (entity.EntityType == EntityType.MAPTILE)
        {
            MapTileCollection.AddEntity(entity as MapTile);
        }
        if (entity.EntityType == EntityType.PLAYER)
        {
            PlayerCollection.AddEntity(entity as Player);
        }
        if (entity.EntityType == EntityType.TERRAIN)
        {
            TerrainCollection.AddEntity(entity as TerrainBase);
        }
    }

    public static Enemy SpawnEnemy(Location location, GameObject enemyPrefab)
    {
        Enemy enemy = (PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject).GetComponent<Enemy>();
        enemy.SetLocation(location, true);
        enemy.SpriteRenderer.flipX = (location.X < 0);
        Entities.EnemyCollection.AddEntity(enemy);
        return enemy;
    }

    //returns null if can't spawn the terran
    public static TerrainBase SpawnTerrain(Location location, GameObject terrainPrefab)
    {
        if (!LocationUtility.HasTerrain(location))
        {
            TerrainBase terrain = (PrefabUtility.InstantiatePrefab(terrainPrefab) as GameObject).GetComponent<TerrainBase>();
            terrain.SetLocation(location, true);
            terrain.GetComponent<SpriteRenderer>().flipX = (location.X < 0);
            Entities.TerrainCollection.AddEntity(terrain);
            return terrain;
        }
        return null;
    }

    private void OnDestroy()
    {
        EnemyCollection.RemoveAll();
        PlayerCollection.RemoveAll();
        MapTileCollection.RemoveAll();
        TerrainCollection.RemoveAll();
    }
    // important code
}

public class Entities<T> where T : GameEntity
{
    HashSet<T> entities = new();

    public HashSet<T> EntitiesSet { get => entities; }

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

    public void RemoveAll()
    {
        for(int i = 0; i < entities.Count; i++)
        {
            RemoveEntity(entities.First());
        }
    }
}

public class TrackedCollection<T>
{
    protected HashSet<T> trackedCollection = new();

    public event Action<T> ElementAdded;
    public event Action<T> ElementRemoved;

    public void Add(T element)
    {
        trackedCollection.Add(element);
        ElementAdded?.Invoke(element);
    }

    public void Remove(T element)
    {
        trackedCollection.Remove(element);
        ElementRemoved?.Invoke(element);
    }
}