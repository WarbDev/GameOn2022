using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridCreator : MonoBehaviour
{
    int topPosition;
    int furthestPosition;

    [SerializeField] int initialReach;
    [SerializeField] int initialHeight;

    public int InitialReach { get => initialReach; }
    public int InitialHeight { get => initialHeight; }

    Dictionary<Location, Dictionary<Location, Location>> grid = new();
    public event Action<IEnumerable<Location>> FinishedCreatingGrid;
    public event Action<Location> AddedNode;

    private void Start()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        AddToGrid((0, 1));
        topPosition = 1;
        furthestPosition = 0;

        ExpandHorizontal(initialReach);
        ExpandVertical(initialHeight - 1);
        FinishedCreatingGrid?.Invoke(grid.Keys.ToList());
    }

    public List<Location> ExpandHorizontal(int amount)
    {
        List<Location> newLocations = new();
        int target = amount + furthestPosition;
        for (int i = furthestPosition + 1; i <= target; i++)
        {
            // Add 2 new columns, symmetrical of one another.
            for (int j = topPosition; j > 0; j--)
            {
                newLocations.Add((i, j));
                AddToGrid((i, j)); // Add right side

                newLocations.Add((-i, j));
                AddToGrid((-i, j)); // Add left side
            }

            // Update the neighbor dictionaries for the previously furthest columns
            for (int k = 1; k <= topPosition; k++)
            {
                grid[(furthestPosition, k)] = UpdateNeighbors((furthestPosition, k));
                grid[(-furthestPosition, k)] = UpdateNeighbors((-furthestPosition, k));
            }

            furthestPosition++;
        }
        return newLocations;
    }

    public List<Location> ExpandVertical(int amount)
    {
        List<Location> newLocations = new List<Location>();
        int target = amount + topPosition;
        for (int i = topPosition + 1; i <= target; i++)
        {
            newLocations.Add((0, i));
            AddToGrid((0, i));
            for (int j = 1; j <= furthestPosition; j++)
            {
                newLocations.Add((j, i));
                AddToGrid((j, i)); // Add right side

                newLocations.Add((-j, i));
                AddToGrid((-j, i)); // Add left side
            }

            for (int k = 0; k <= furthestPosition; k++)
            {
                grid[(k, topPosition)] = UpdateNeighbors((k, topPosition));
                grid[(-k, topPosition)] = UpdateNeighbors((-k, topPosition));
            }

            topPosition++;
        }
        return newLocations;
    }

    void AddToGrid(Location location)
    {
        grid.Add(location, UpdateNeighbors(location));
        AddedNode?.Invoke(location);
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
