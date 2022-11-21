using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTerrain : TerrainBase
{
    Location location;

    public GameObject GameObject { get => gameObject; }

    public override Location Location { get => location; protected set => location = value; }

    public override EntityType EntityType { get => EntityType.TERRAIN; }

    public override void DestroyEntity()
    {
        Destroy(gameObject);
    }

    public void OnEntityMoveOver()
    {

    }
}
