using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TerrainBase : GameEntity, IAnimatable, ITickable
{
    [SerializeField] AnimatableEntity terrainAnimations;
    [SerializeField] SpriteRenderer spriteRenderer;
    public IAnimatable Animatable { get => terrainAnimations; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer;}

    public abstract void OnRoundTick();
    public abstract Func<bool> OnEntityMoveOver(GameEntity entity);
}
