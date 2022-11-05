using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMap : MonoBehaviour, IObserveEnemies, IObservePlayers, IObserveTiles
{
    public static Dictionary<Location, List<Location>> NeighborsDictionary;
    public static HashSet<Location> AllPositions;
    public static Dictionary<Location, Dictionary<(int, int), Location>> DirectionalNeighborsDictionary; // Optimized for searching through specific directions
    public static Dictionary<Location, IGameEntity> MapTilesDictionary = new();
    public static Dictionary<Location, IGameEntity> PlayersDictionary = new();
    public static Dictionary<Location, IGameEntity> EnemiesDictionary = new();

    public static Location BottomLeft;
    public static Location BottomRight;
    public static Location TopLeft;
    public static Location TopRight;


    void Awake()
    {
        PlayerCollection.PlayerAdded += OnPlayerAdded;
        PlayerCollection.PlayerRemoved += OnPlayerRemoved;

        EnemyCollection.EnemyAdded += OnEnemyAdded;
        EnemyCollection.EnemyRemoved += OnEnemyRemoved;

        MapTileCollection.MapTileAdded += OnTileAdded;
        MapTileCollection.MapTileRemoved += OnTileRemoved;
    }

    # region GameEntity Adding/Removal
    public void OnEnemyAdded(Enemy enemy)
    {
        EnemiesDictionary.Add(enemy.Location, enemy);
    }

    public void OnEnemyRemoved(Enemy enemy)
    {
        EnemiesDictionary.Remove(enemy.Location);
    }

    public void OnPlayerAdded(Player player)
    {
        PlayersDictionary.Add(player.Location, player);
    }

    public void OnPlayerRemoved(Player player)
    {
        EnemiesDictionary.Remove(player.Location);
    }

    public void OnTileAdded(MapTile tile)
    {
        MapTilesDictionary.Add(tile.Location, tile);
    }

    public void OnTileRemoved(MapTile tile)
    {
        MapTilesDictionary.Remove(tile.Location);
    }
    #endregion
}