using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMap : MonoBehaviour, IObserveEnemies, IObservePlayers, IObserveTiles
{
    [SerializeField] GridCreator gridCreator;
    public static bool HasTwoFronts;
    public static Dictionary<Location, List<Location>> NeighborsDictionary = new();
    public static Dictionary<Location, IGameEntity> MapTilesDictionary = new();
    public static Dictionary<Location, IGameEntity> PlayersDictionary = new();
    public static Dictionary<Location, IGameEntity> EnemiesDictionary = new();

    public static int LeftBorder;
    public static int RightBorder;
    public static int TopBorder;
    public static int BottomBorder;


    void Awake()
    {
        TopBorder = gridCreator.InitialHeight;
        BottomBorder = 1;

        RightBorder = gridCreator.InitialReach;
        LeftBorder = 0;

        PlayerCollection.PlayerAdded += OnPlayerAdded;
        PlayerCollection.PlayerRemoved += OnPlayerRemoved;

        EnemyCollection.EnemyAdded += OnEnemyAdded;
        EnemyCollection.EnemyRemoved += OnEnemyRemoved;

        MapTileCollection.MapTileAdded += OnTileAdded;
        MapTileCollection.MapTileRemoved += OnTileRemoved;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            IncreaseLength(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseHeight(1);
        }
    }

    public void IncreaseLength(int amount)
    {
        RightBorder += amount;
        if (HasTwoFronts)
        {
            LeftBorder = -RightBorder;
        }
        gridCreator.ExpandHorizontal(amount);
    }

    public void IncreaseHeight(int amount)
    {
        TopBorder += amount;
        gridCreator.ExpandVertical(amount);
    }

    public static void MovePlayer(IGameEntity player, Location to)
    {
        if (PlayersDictionary.ContainsKey(to))
        {
            throw new Exception("Trying to move to a player to an occupied position! Do SwapPlayers instead!");
        }

        Location oldLocation = player.Location;
        PlayersDictionary.Remove(oldLocation);
        PlayersDictionary.Add(to, player);

        player.Location = to;
    }

    public static void SwapPlayers(IGameEntity player1, IGameEntity player2)
    {
        Location location1 = player1.Location;
        Location location2 = player2.Location;

        PlayersDictionary[location1] = player2;
        PlayersDictionary[location2] = player1;

        Location tempLocation = new Location(player1.Location.X, player1.Location.Y);

        player1.Location = new Location(player2.Location.X, player2.Location.Y);
        player2.Location = tempLocation;
    }

    public static void MoveEnemy(IGameEntity enemy, Location to)
    {
        Location oldLocation = enemy.Location;
        EnemiesDictionary.Remove(oldLocation);
        EnemiesDictionary.Add(to, enemy);
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