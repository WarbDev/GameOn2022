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

    Dictionary<Location, Dictionary<Location, Location>> grid = new();
    public event Action<IEnumerable<Location>> FinishedCreatingGrid;

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

    void ExpandHorizontal(int amount)
    {
        int target = amount + furthestPosition;
        for (int i = furthestPosition + 1; i <= target; i++)
        {
            // Add 2 new columns, symmetrical of one another.
            for (int j = topPosition; j > 0; j--)
            {
                AddToGrid((i, j)); // Add right side
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
    }

    void ExpandVertical(int amount)
    {
        int target = amount + topPosition;
        for (int i = topPosition + 1; i <= target; i++)
        {
            AddToGrid((0, i));
            for (int j = 1; j <= furthestPosition; j++)
            {
                AddToGrid((j, i)); // Add right side
                AddToGrid((-j, i)); // Add left side
            }

            for (int k = 0; k <= furthestPosition; k++)
            {
                grid[(k, topPosition)] = UpdateNeighbors((k, topPosition));
                grid[(-k, topPosition)] = UpdateNeighbors((-k, topPosition));
            }

            topPosition++;
        }
    }

    void AddToGrid(Location location)
    {
        grid.Add(location, UpdateNeighbors(location));
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
