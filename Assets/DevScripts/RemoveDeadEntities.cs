using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveDeadEntities : MonoBehaviour
{
    HashSet<IGameEntity> deadEntities = new HashSet<IGameEntity>();
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

    void AddToDeathPool(IGameEntity entity)
    {
        deadEntities.Add(entity);
    }

    void RemoveFromDeathPool(IGameEntity entity)
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
            Entities.RemoveEntity(entity);
            entity.DestroyEntity();
            
        }
        deadEntities.Clear();
    }
}
