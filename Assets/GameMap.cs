using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public static Dictionary<Location, List<Location>> NeighborsDictionary;
    public static HashSet<Location> AllPositions;
    public static Dictionary<Location, Dictionary<(int, int), Location>> DirectionalNeighborsDictionary; // Optimized for searching through specific directions
    public static Dictionary<Location, MapTile> MapTilesDictionary;
    public static Dictionary<Location, IGameEntity> PlayersDictionary = new();
    public static Dictionary<Location, IGameEntity> EnemiesDictionary = new();

    public static Location BottomLeft;
    public static Location BottomRight;
    public static Location TopLeft;
    public static Location TopRight;
}