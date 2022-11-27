using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainBase : GameEntity, IAnimatable
{
    [SerializeField] AnimatableEntity terrainAnimations;
    public IAnimatable Animatable { get => terrainAnimations; }

    public abstract void OnRoundEnd();
    public abstract void OnEntityMoveOver(GameEntity entity);
}
