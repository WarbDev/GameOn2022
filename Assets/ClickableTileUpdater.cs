using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTileUpdater : MonoBehaviour, IObserveTiles
{
    [SerializeField] ClickListener clickListener;
    void Awake()
    {
        MapTileCollection.MapTileAdded += OnTileAdded;
        MapTileCollection.MapTileRemoved += OnTileRemoved;
    }

    public void OnTileAdded(MapTile Tile)
    {
        clickListener.AddClickable(Tile.GetComponent<ClickableEntity>());
    }

    public void OnTileRemoved(MapTile Tile)
    {
        clickListener.RemoveClickable(Tile.GetComponent<ClickableEntity>());
    }
}