using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTerrain : MonoBehaviour, ITerrain
{
    Location location;

    public GameObject GameObject { get => gameObject; }

    public Location Location { get => location; set => location = value; }

    public EntityType EntityType { get => EntityType.TERRAIN; }

    public void DestroyEntity()
    {
        Destroy(gameObject);
    }

    public bool IsObstructedBy(IGameEntity entity)
    {
        throw new System.NotImplementedException();
    }

    public void OnEntityMoveOver()
    {

    }

}
