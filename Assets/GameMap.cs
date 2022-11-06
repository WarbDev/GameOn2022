using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMap : MonoBehaviour
{
    [SerializeField] GridCreator gridCreator;

    #region Map Properties
    [SerializeField] bool hasTwoFronts;
    public bool HasTwoFronts { get => hasTwoFronts; }

    static int leftBorder;
    static int rightBorder;
    static int topBorder;
    static int bottomBorder;

    public static int LeftBorder { get => leftBorder; private set => leftBorder = value; }
    public static int RightBorder { get => rightBorder; private set => rightBorder = value; }
    public static int TopBorder { get => topBorder; private set => topBorder = value; }
    public static int BottomBorder { get => bottomBorder; private set => bottomBorder = value; }
    #endregion

    #region GameEntity Dictionaries
    static Dictionary<Location, List<Location>> neighborsDictionary = new();
    static Dictionary<Location, IGameEntity> mapTilesDictionary = new();
    static Dictionary<Location, IGameEntity> playersDictionary = new();
    static Dictionary<Location, IGameEntity> enemiesDictionary = new();
    static Dictionary<EntityType, Dictionary<Location, IGameEntity>> typeDictionary = new();

    public static Dictionary<Location, List<Location>> NeighborsDictionary
    {
        get => neighborsDictionary; 
        private set => neighborsDictionary = value; 
    }

    public static Dictionary<Location, IGameEntity> MapTilesDictionary
    {
        get => mapTilesDictionary;
        private set => mapTilesDictionary = value; 
    }

    public static Dictionary<Location, IGameEntity> PlayersDictionary
    {
        get => playersDictionary;
        private set => playersDictionary = value;
    }

    public static Dictionary <Location, IGameEntity> EnemiesDictionary
    {
        get => enemiesDictionary;
        private set => enemiesDictionary = value;
    }
    #endregion


    void Awake()
    {
        typeDictionary.Add(EntityType.ENEMY, EnemiesDictionary);
        typeDictionary.Add(EntityType.PLAYER, PlayersDictionary);
        typeDictionary.Add(EntityType.MAPTILE, MapTilesDictionary);

        TopBorder = gridCreator.InitialHeight;
        BottomBorder = 1;

        RightBorder = gridCreator.InitialReach;
        LeftBorder = 0;
        if (HasTwoFronts)
        {
            LeftBorder = -RightBorder;
        }

        EntityCollection.PlayerCollection.EntityAdded += OnEntityAdded;
        EntityCollection.PlayerCollection.EntityRemoved += OnEntityRemoved;

        EntityCollection.EnemyCollection.EntityAdded += OnEntityAdded;
        EntityCollection.EnemyCollection.EntityRemoved += OnEntityRemoved;

        EntityCollection.MapTileCollection.EntityAdded += OnEntityAdded;
        EntityCollection.MapTileCollection.EntityRemoved += OnEntityRemoved;
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
        enemy.Location = to;
        EnemiesDictionary.Add(to, enemy);
    }

    void OnEntityAdded(IGameEntity entity)
    {
        typeDictionary[entity.EntityType].Add(entity.Location, entity);
    }
    void OnEntityRemoved(IGameEntity entity)
    {
        typeDictionary[entity.EntityType].Remove(entity.Location);
    }
}