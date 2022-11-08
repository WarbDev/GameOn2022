using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesWithHealthCollection : MonoBehaviour
{
    static TrackedCollection<IHaveHealth> entitiesWithHealth = new();
    public static TrackedCollection<IHaveHealth> EntitiesWithHealth { get => entitiesWithHealth; }

    private void Awake()
    {
        Entities.EnemyCollection.EntityAdded += AddDamageable;
        Entities.PlayerCollection.EntityAdded += AddDamageable;

        Entities.EnemyCollection.EntityRemoved += RemoveDamageable;
        Entities.PlayerCollection.EntityRemoved += RemoveDamageable;
    }

    void AddDamageable(IHaveHealth damageable)
    {
        entitiesWithHealth.Add(damageable);
    }

    void RemoveDamageable(IHaveHealth damageable)
    {
        entitiesWithHealth.Remove(damageable);
    }


}