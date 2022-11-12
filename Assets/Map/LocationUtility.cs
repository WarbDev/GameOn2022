using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class LocationUtility
{
    public static List<Location> Neighbors(Location l)
    {
        return GameMap.NeighborsDictionary[l];
    }

    public static List<List<Location>> GetEndColumns()
    {
        List<List<Location>> list = new(2);
        list.Add(GetLeftEndColumn());
        list.Add(GetRightEndColumn());
        return list;
    }

    public static List<Location> GetRightEndColumn()
    {
        if (GameMap.RightBorder != 0)
        {
            return GetColumn(GameMap.RightBorder);
        }
        return new List<Location>();
    }

    public static List<Location> GetLeftEndColumn()
    {
        if (GameMap.LeftBorder != 0)
        {
            return GetColumn(GameMap.LeftBorder);
        }
        return new List<Location>();
    }

    public static List<Location> GetColumn(int x)
    {
        List<Location> columns = new();
        if (IsOnMap((x, 1)))
        {
            for (int i = 1; i <= GameMap.TopBorder; i++)
            {
                columns.Add((x, i));
            }
        }
        return columns;
    }

    //Gives all of the locations on the opposite side of the 
    //Center Player line, as if mirrored
    public static List<Location> FlipLocations(List<Location> locations)
    {
        List<Location> flippedLocations = new();
        foreach(Location loc in locations)
        {
            flippedLocations.Add(new Location(loc.X * -1, loc.Y));
        }
        return flippedLocations;
    }

    public static List<Location> RemoveOffMapLocations(IEnumerable<Location> locations)
    {
        List<Location> filteredLocations = new();
        filteredLocations = locations.Where(location => IsOnMap(location)).ToList();
        return filteredLocations;
    }

    public static bool IsOnMap(Location location)
    {
        return (GameMap.LeftBorder <= location.X && location.X <= GameMap.RightBorder 
            && GameMap.BottomBorder <= location.Y && location.Y <= GameMap.TopBorder);
    }

    public static List<Location> LocationsInSquareRadius(Location l, int range)
    {
        List<Location> square = new();
        square.Add(l);
        for (int i = 1; i <= range; i++)
        {
            square.AddRange(LocationsInLine((l + new Location(Directions.NW) * i), 2 * i - 1, Directions.E));
            square.AddRange(LocationsInLine((l + new Location(Directions.NE) * i), 2 * i - 1, Directions.S));
            square.AddRange(LocationsInLine((l + new Location(Directions.SE) * i), 2 * i - 1, Directions.W));
            square.AddRange(LocationsInLine((l + new Location(Directions.SW) * i), 2 * i - 1, Directions.N));
        }

        return square;
    }

    public static List<Location> LocationsInLine(Location l, int range, (int, int) direction)
    {
        List<Location> locationsInLine = new();
        for (int i = 0; i <= range; i++)
        {
            Location furtherLocation = l + new Location(direction) * i;
            locationsInLine.Add(furtherLocation);
        }
        return locationsInLine;
    }

    public static bool TryGetPlayer(Location location, out Player player)
    {
        bool hasPlayer = GameMap.PlayersDictionary.TryGetValue(location, out IGameEntity playerEntity);
        if (hasPlayer)
            player = playerEntity as Player;
        else
            player = null;
        return hasPlayer;
    }
    public static bool TryGetEnemy(Location location, out Enemy enemy)
    {
        bool hasEnemy = GameMap.EnemiesDictionary.TryGetValue(location, out IGameEntity enemyEntity);
        if (hasEnemy)
            enemy = enemyEntity as Enemy;
        else
            enemy = null;
        return hasEnemy;
    }
    public static List<Enemy> GetEnemiesInPositions(List<Location> positions)
    {
        List<Enemy> enemies = new List<Enemy>();
        foreach (var pos in positions)
        {
            Enemy enemy;
            bool hasEnemy = LocationUtility.TryGetEnemy(pos, out enemy);
            if (hasEnemy)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }

    public static bool TryGetTile(Location location, out MapTile tile)
    {
        bool hasTile = GameMap.MapTilesDictionary.TryGetValue(location, out IGameEntity tileEntity);
        if (hasTile)
            tile = tileEntity as MapTile;
        else
            tile = null;
        return tile;
    }
    public static List<MapTile> GetTilesInPositions(List<Location> positions)
    {
        List<MapTile> tiles = new List<MapTile>();
        foreach (var pos in positions)
        {
            MapTile tile;
            bool hasTile = LocationUtility.TryGetTile(pos, out tile);
            if (hasTile)
            {
                tiles.Add(tile);
            }
        }
        return tiles;
    }

    public static Vector3 LocationToVector3(Location loc)
    {
        float scale = 1;
        return new Vector3(loc.X * scale, loc.Y * scale);
    }
}
