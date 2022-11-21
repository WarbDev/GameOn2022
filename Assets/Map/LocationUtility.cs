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

    // Generates all columns,
    // first generates all right columns from center->left
    // then generates all right columns from center->right
    public static IEnumerable<List<Location>> AllColumns()
    {
        for (int i = -1; i >= GameMap.LeftBorder; i--)
        {
            yield return LocationUtility.GetColumn(i);
        }

        for (int i = 1; i <= GameMap.RightBorder; i++)
        {
            yield return LocationUtility.GetColumn(i);
        }
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

    public static bool IsOccupied(Location location)
    {
        if (HasPlayer(location) || HasEnemy(location))
        {
            return true;
        }
        return false;
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
        bool hasPlayer = GameMap.PlayersDictionary.TryGetValue(location, out GameEntity playerEntity);
        if (hasPlayer)
            player = playerEntity as Player;
        else
            player = null;
        return hasPlayer;
    }

    public static bool HasPlayer(Location location)
    {
        return GameMap.PlayersDictionary.ContainsKey(location);
    }

    public static bool TryGetEnemy(Location location, out Enemy enemy)
    {
        bool hasEnemy = GameMap.EnemiesDictionary.TryGetValue(location, out GameEntity enemyEntity);
        if (hasEnemy)
            enemy = enemyEntity as Enemy;
        else
            enemy = null;
        return hasEnemy;
    }

    public static bool HasEnemy(Location location)
    {
        return GameMap.EnemiesDictionary.ContainsKey(location);
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
        bool hasTile = GameMap.MapTilesDictionary.TryGetValue(location, out GameEntity tileEntity);
        if (hasTile)
            tile = tileEntity as MapTile;
        else
            tile = null;
        return tile;
    }

    public static bool HasMapTile(Location location)
    {
        return GameMap.MapTilesDictionary.ContainsKey(location);
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

    
    public static List<GameEntity> GetEntitiesAtPosition(Location position)
    {
        var entities = new List<GameEntity>();
        foreach (var dic in GameMap.AllEntityDictionaries)
        {
            if (dic.ContainsKey(position))
            {
                entities.Add(dic[position]);
            }
        }
        return entities;
    }

    public static List<IObstructingEntity> GetObstructionsAtPosition(Location position)
    {
        var entities = GetEntitiesAtPosition(position);
        var obstructions = new List<IObstructingEntity>();
        foreach (var entity in entities)
        {
            var obstructingEntity = entity as IObstructingEntity;
            if (obstructingEntity != null)
            {
                obstructions.Add(obstructingEntity);
            }
        }
        return obstructions;
    }

    public static Location CalculateRelativeLocationFromDirectionAndMagnitude(Location direction, int magnitude)
    {
        if (direction.X == 0 && direction.Y == 0)
        {
            return direction;
        }
        float maxDistance = Mathf.Max(Mathf.Abs(direction.X), Mathf.Abs(direction.Y));
        float proportionalMagnitude = (float) magnitude / maxDistance;
        float x = direction.X * proportionalMagnitude;
        float y = direction.Y * proportionalMagnitude;
        Location endPoint = new Location((int) Mathf.Round(x), (int) Mathf.Round(y));
        return endPoint;
    }

    public static Vector3 LocationToVector3(Location loc)
    {
        float scale = 1;
        return new Vector3(loc.X * scale, loc.Y * scale);
    }

    // Takes two lists and places the elements of each list one after another as many times as possible.
    // If one list has fewer elements than the other, then the remaining elements of the longer list will
    // be simply all added at the end.
    static List<T> Interweave<T>(ref List<T> list1, ref List<T> list2) where T : IList
    {
        List<T> interweavedList = new();
        int highestCount = list1.Count;
        if (list1.Count < list2.Count)
        {
            highestCount = list2.Count;
        }

        for (int i = 0; i < highestCount; i++)
        {
            if (i < list1.Count)
            {
                interweavedList.Add(list1[i]);
            }

            if (i < list2.Count)
            {
                interweavedList.Add(list2[i]);
            }
        }
        return interweavedList;
    }
}
