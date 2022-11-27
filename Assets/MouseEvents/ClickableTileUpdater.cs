using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTileUpdater : MonoBehaviour
{
    [SerializeField] ClickListener clickListener;
    void Awake()
    {
        Entities.MapTileCollection.EntityAdded += OnTileAdded;
        Entities.MapTileCollection.EntityRemoved += OnTileRemoved;
    }

    void OnDestroy()
    {
        Entities.MapTileCollection.EntityAdded -= OnTileAdded;
        Entities.MapTileCollection.EntityRemoved -= OnTileRemoved;
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