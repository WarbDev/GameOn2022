using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridCreator : MonoBehaviour
{

    Dictionary<Location, Dictionary<Location, Location>> grid = new();
    public event Action<Location> AddedNode;

    private void Awake()
    {
        GameMap.MapExpanded += AddToGrid;
    }

    void AddToGrid(Location location)
    {
        grid.Add(location, UpdateNeighbors(location));
        AddedNode?.Invoke(location);
    }

    void AddToGrid(List<Location> locations)
    {
        foreach(var location in locations)
        {
            AddToGrid(location);
        }
    }

    Dictionary<Location, Location> UpdateNeighbors(Location location)
    {
        Dictionary<Location, Location> neighbors = new();

        foreach (var dir in Directions.All)
        {
            if (grid.ContainsKey(location + dir))
            {
                neighbors.Add(dir, location + dir);
            }
        }
        return neighbors;
    }
}
