using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity, IHaveHealth
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.PLAYER; }
    public Health Health { get => health; set => health = value; }

    public DamageableHealth DamageableHealth { get => damageableHealth; set => damageableHealth = value; }
    [SerializeField] DamageableHealth damageableHealth;

    public GameObject GameObject { get => gameObject; }

    public string Name;

    [SerializeField] Health health;
    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
