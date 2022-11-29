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

    static Dictionary<EntityType, IEntityCollection> Collections = new() { 
        { EntityType.ENEMY, EnemyCollection },
        { EntityType.PLAYER, PlayerCollection },
        { EntityType.MAPTILE, MapTileCollection },
        { EntityType.TERRAIN, TerrainCollection }
    };

    public static void RemoveEntity(GameEntity entity)
    {
        Collections[entity.EntityType].RemoveEntity(entity);
    }

    public static void AddEntity(GameEntity entity)
    {
        Collections[entity.EntityType].AddEntity(entity);
    }

    public static bool HasEntity(GameEntity entity)
    {
        return Collections[entity.EntityType].HasEntity(entity);
    }

    public static Enemy SpawnEnemy(Location location, GameObject enemyPrefab)
    {
        //Enemy enemy = (PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject).GetComponent<Enemy>();
        Enemy enemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
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
            //TerrainBase terrain = (PrefabUtility.InstantiatePrefab(terrainPrefab) as GameObject).GetComponent<TerrainBase>();
            TerrainBase terrain = Instantiate(terrainPrefab).GetComponent<TerrainBase>();
            terrain.SetLocation(location, true);
            terrain.SpriteRenderer.flipX = (location.X < 0);
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

public class Entities<T> : IEntityCollection where T : GameEntity
{
    HashSet<GameEntity> entities = new();

    public HashSet<GameEntity> EntitiesSet { get => entities; }

    public event Action<T> EntityAdded;
    public event Action<T> EntityRemoved;

    public void AddEntity(GameEntity entity)
    {
        entities.Add(entity);
        EntityAdded?.Invoke(entity as T);
    }

    public void RemoveEntity(GameEntity entity)
    {
        entities.Remove(entity);
        EntityRemoved?.Invoke(entity as T);
    }

    public bool HasEntity(GameEntity entity)
    {
        return entities.Contains(entity);
    }

    public void RemoveAll()
    {
        for(int i = 0; i < entities.Count; i++)
        {
            RemoveEntity(entities.First());
        }
    }
}

public interface IEntityCollection
{
    public void AddEntity(GameEntity entity);
    public void RemoveEntity(GameEntity entity);
    public bool HasEntity(GameEntity entity);
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