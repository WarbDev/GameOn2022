using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IGameEntity, IDamageable, IHaveHealth
{
    Location location;
    public Location Location { get => location; set => location = value; }
    public EntityType EntityType { get => EntityType.PLAYER; }
    public Health Health { get => health; set => health = value; }

    public GameObject GameObject { get => gameObject; }

    public string Name;

    [SerializeField] Health health;

    public DamageLog DealDamage(DamageDetails damage)
    {
        float oldHealth = Health.CurrentHealth;
        float newHealth = Health.ReduceHealth(damage.Base);
        return new DamageLog(this, oldHealth, newHealth, damage);
    }
    public void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
