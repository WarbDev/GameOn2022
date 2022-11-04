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

    public static List<Location> RemoveOffMapLocations(IEnumerable<Location> locations)
    {
        List<Location> filteredLocations = new();
        filteredLocations = locations.Where(location => GameMap.AllPositions.Contains(location)).ToList();
        return filteredLocations;
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
}
