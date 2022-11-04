using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserveTiles
{
    public void OnTileAdded(MapTile tile);
    public void OnTileRemoved(MapTile tile);
}
