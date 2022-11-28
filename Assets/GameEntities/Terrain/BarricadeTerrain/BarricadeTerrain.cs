using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarricadeTerrain : TerrainBase, IDamageable, IObstruct
{
    Location location;

    public override Location Location { get => location; protected set => location = value; }

    public override EntityType EntityType { get => EntityType.TERRAIN; }

    [SerializeField] Obstruction obstruction;
    [SerializeField] DamageableComponent damageable;
    public bool IsDead { get => damageable.IsDead; }
    public IObstruct Obstruct { get => obstruction; }

    public IDamageable Damageable { get => damageable; }

    public override Func<bool> OnEntityMoveOver(GameEntity entity)
    {
        return () => true; // pp
    }

    public override void OnRoundTick()
    {

    }
}
