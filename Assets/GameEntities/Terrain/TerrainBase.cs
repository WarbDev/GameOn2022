using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainBase : GameEntity
{
    public abstract void OnEntityMoveOver(GameEntity entity);
}
