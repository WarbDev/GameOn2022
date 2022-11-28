using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : GameEntity
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.MAPTILE; }
    public GameObject GameObject { get => gameObject; }

    public override void RemoveFromEntityTracker()
    {
        Destroy(gameObject);
    }

    public override void SetLocation(Location newLocation)
    {
        Location = newLocation;
    }

    public override void SetLocation(Location newLocation, bool isInitializing)
    {
        Location = newLocation;
    }

    public bool IsObstructedBy(GameEntity entity)
    {
        Debug.Log("IsObstructedBy IS NOT IMPLIMENTED");
        return false;
    }
}
