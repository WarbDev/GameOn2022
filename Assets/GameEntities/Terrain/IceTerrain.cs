using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTerrain : TerrainBase, IDamageable, IObstruct
{
    Location location;

    public override Location Location { get => location; protected set => location = value; }

    public override EntityType EntityType {get => EntityType.TERRAIN;}

    [SerializeField] Obstruction obstruction;
    [SerializeField] DamageableComponent damageable;

    public IObstruct Obstruct { get => obstruction; }

    public IDamageable Damageable { get => damageable; }

    public int TurnsUntilExtinguish;

    public override void OnEntityMoveOver(GameEntity entity)
    {
        
    }

    public override void OnRoundEnd()
    {
        TurnsUntilExtinguish--;
        if (TurnsUntilExtinguish <= 0)
        {
            DestroyEntity();
        }
    }

}
