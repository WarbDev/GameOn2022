using System.Collections.Generic;
using System;

public static class MapTileCollection
{
    private static HashSet<MapTile> mapTiles = new();

    public static event Action<MapTile> MapTileAdded;
    public static event Action<MapTile> MapTileRemoved;

    public static void AddMapTile(MapTile mapTile)
    {
        mapTiles.Add(mapTile);
        MapTileAdded?.Invoke(mapTile);
    }

    public static void RemoveMapTile(MapTile mapTile)
    {
        mapTiles.Remove(mapTile);
        MapTileRemoved?.Invoke(mapTile);
    }
}
