using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour, IGameEntity
{
    Location location;
    public Location Location
    {
        get
        {
            return location;
        }
        set
        {
            location = value;
            
        }
    }

    
}
