using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveDeadEntities : MonoBehaviour
{
    HashSet<GameEntity> deadEntities = new HashSet<GameEntity>();
    private void Start()
    {
        Entities.PlayerCollection.EntityRemoved += RemoveFromDeathPool;
        Entities.EnemyCollection.EntityRemoved += RemoveFromDeathPool;
        Health.LostAllHealth += AddToDeathPool;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            DeleteEntitiesInDeadPool();
        }
    }

    void AddToDeathPool(GameEntity entity)
    {
        deadEntities.Add(entity);
    }

    void RemoveFromDeathPool(GameEntity entity)
    {
        if (deadEntities.Contains(entity))
        {
            deadEntities.Remove(entity);
        }
    }

    public void DeleteEntitiesInDeadPool()
    {

        foreach (var entity in deadEntities.ToList())
        {
            entity.DestroyEntity();
        }

        deadEntities.Clear();
    }
}
