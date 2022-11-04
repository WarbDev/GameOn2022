using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour, IGameEntity
{
    [SerializeField] int X;
    [SerializeField] int Y;
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
            X = location.X;
            Y = location.Y;
            
        }
    }

    
}
