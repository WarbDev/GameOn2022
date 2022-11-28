using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainBase : GameEntity, IAnimatable, ITickable
{
    [SerializeField] AnimatableEntity terrainAnimations;
    public IAnimatable Animatable { get => terrainAnimations; }

    public abstract void OnRoundTick();
    public abstract void OnEntityMoveOver(GameEntity entity);
}
