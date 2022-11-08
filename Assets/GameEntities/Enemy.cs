using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGameEntity, ICanMakeMoveRequests, IDamageable, IHaveHealth
{
    Location location;

    public Location Location { get => location; set => location = value;}
    public EntityType EntityType { get => EntityType.ENEMY; }
    public Health Health { get => health; set => health = value; }

    [SerializeField] Health health;
    public GameObject GameObject { get => gameObject; }

    public MoveLog DoTurnMovement()
    {
        if (LocationUtility.TryGetEnemy((Location.X + DirectionTowardsPlayers(), Location.Y), out Enemy enemy))
        {
            return null;
        }
        Location newLocation = (Location.X + DirectionTowardsPlayers(), Location.Y);
        MoveLog moveLog = new MoveLog(this, location, newLocation);
        GameMap.MoveEnemy(this, newLocation);
        return moveLog;
    }

    public void OnMouseDown()
    {
        var damageDetails = new DamageDetails();
        damageDetails.DamageSource = this;
        damageDetails.Base = 2f;
        Debug.Log("Dealt damage!");
        DamageEffect.Apply(this, damageDetails);
    }

    public void DestroyEntity()
    {
        Destroy(gameObject);
    }

    int DirectionTowardsPlayers()
    {
        return System.Math.Sign(Location.X * -1);
    }
    

    public DamageLog DealDamage(DamageDetails damage)
    {
        float oldHealth = Health.CurrentHealth;
        float newHealth = Health.ReduceHealth(damage.Base);
        return new DamageLog(this, oldHealth, newHealth, damage);
    }
}
