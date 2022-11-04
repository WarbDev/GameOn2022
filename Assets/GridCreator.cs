using System;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] int mapLength;
    [SerializeField] int mapHeight;
    [SerializeField] bool hasTwoFronts;

    HashSet<Location> allPositions;
    Dictionary<Location, List<Location>> neighborsDictionary;
    Dictionary<Location, Dictionary<(int, int), Location>> neighborsDirectionalDictionary;

    Location topLeft;
    Location topRight;
    Location bottomLeft;
    Location bottomRight;

    public bool Finished = false;

    
    public void CreateGrid()
    {
        InitializePositions(mapLength, mapHeight, hasTwoFronts);
        Finished = true;
    }

    void InitializePositions(int length, int height, bool twoFronts)
    {
        Location mapDimensions = (length, height);
        allPositions = new();
        int startL = 0;

        if (twoFronts)
            startL = -mapDimensions.X;

        bottomLeft = (startL, 1);
        bottomRight = (length, 1);
        topLeft = (startL, height);
        topRight = (length, height);

        GameMap.TopLeft = topLeft;
        GameMap.BottomLeft = bottomLeft;
        GameMap.BottomRight = bottomRight;
        GameMap.TopRight = topRight;

        for (int i = startL; i <= mapDimensions.X; i++)
        {
            for (int j = 1; j <= mapDimensions.Y; j++)
            {
                allPositions.Add((i, j));
            }
        }
        GameMap.AllPositions = allPositions;
        DefineNeighbors();
    }

    void DefineNeighbors()
    {
        neighborsDictionary = new Dictionary<Location, List<Location>>();
        neighborsDirectionalDictionary = new();
        foreach (var l in allPositions)
        {
            neighborsDictionary.Add(l, GetNeighbors(l));
            neighborsDirectionalDictionary.Add(l, GetNeighborsDirectionalDictionary(l));
        }
        GameMap.NeighborsDictionary = neighborsDictionary;
        GameMap.DirectionalNeighborsDictionary = neighborsDirectionalDictionary;

        List<Location> GetNeighbors(Location location)
        {
            List<Location> neighbors = new List<Location>();
            foreach (var dir in Directions.All)
            {
                Location summedDirWithLoc = (location + dir);
                if (allPositions.Contains(summedDirWithLoc))
                    neighbors.Add(summedDirWithLoc);
                    
            }
            return neighbors;
        }

        Dictionary<(int, int),Location> GetNeighborsDirectionalDictionary(Location location)
        {
            Dictionary<(int, int), Location> directionalNeighbors = new();
            foreach (var dir in Directions.All)
            {
                Location summedDirWithLoc = (location + dir);
                if (allPositions.Contains(summedDirWithLoc))
                    directionalNeighbors.Add(dir, summedDirWithLoc);
            }
            return directionalNeighbors;
        }
    }
}
