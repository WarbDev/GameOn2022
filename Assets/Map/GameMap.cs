using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameMap : MonoBehaviour
{
    public static event Action<List<Location>> MapExpanded;

    #region Map Properties
    [SerializeField] bool hasTwoFronts;

    [SerializeField] int initialRight;
    [SerializeField] int initialLeft;
    [SerializeField] int initialHeight;
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
        DOTween.SetTweensCapacity(500, 312);
        typeDictionary.Clear();

        neighborsDictionary = new();
        mapTilesDictionary = new();
        playersDictionary = new();
        enemiesDictionary = new();

        typeDictionary.Add(EntityType.ENEMY, EnemiesDictionary);
        typeDictionary.Add(EntityType.PLAYER, PlayersDictionary);
        typeDictionary.Add(EntityType.MAPTILE, MapTilesDictionary);

        BottomBorder = 1;

        Entities.PlayerCollection.EntityAdded += OnEntityAdded;
        Entities.PlayerCollection.EntityRemoved += OnEntityRemoved;

        Entities.EnemyCollection.EntityAdded += OnEntityAdded;
        Entities.EnemyCollection.EntityRemoved += OnEntityRemoved;

        Entities.MapTileCollection.EntityAdded += OnEntityAdded;
        Entities.MapTileCollection.EntityRemoved += OnEntityRemoved;
    }

    private void OnDestroy()
    {
        Entities.PlayerCollection.EntityAdded -= OnEntityAdded;
        Entities.PlayerCollection.EntityRemoved -= OnEntityRemoved;

        Entities.EnemyCollection.EntityAdded -= OnEntityAdded;
        Entities.EnemyCollection.EntityRemoved -= OnEntityRemoved;

        Entities.MapTileCollection.EntityAdded -= OnEntityAdded;
        Entities.MapTileCollection.EntityRemoved -= OnEntityRemoved;
    }

    private void Start()
    {
        StartCoroutine(InitializeMap());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ExpandLeft(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ExpandRight(1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ExpandUp(1);
        }
    }

    IEnumerator InitializeMap()
    {
        Location start = new(0, 1);
        topBorder = 1;
        leftBorder = 0;
        rightBorder = 0;
        MapExpanded?.Invoke(new List<Location> { start });

        
        for (int i = 1; i < initialHeight; i++)
        {
            ExpandUp(1);
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 1; i < initialRight; i++)
        {
            ExpandRight(1);
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 1; i < Math.Abs(initialLeft); i++)
        {
            ExpandLeft(1);
            yield return new WaitForSeconds(0.05f);
        }
        StopAllCoroutines();
    }

    void ExpandRight(int amount)
    {
        (List<Location>, int) expansion;
        expansion = ExpandHorizontal(amount, 1);
        rightBorder = expansion.Item2;
        MapExpanded?.Invoke(expansion.Item1);
    }
    void ExpandLeft(int amount)
    {
        (List<Location>, int) expansion;
        expansion = ExpandHorizontal(amount, -1);
        leftBorder = expansion.Item2;
        MapExpanded?.Invoke(expansion.Item1);

    }

    (List<Location>, int) ExpandHorizontal(int amount, int direction)
    {
        int border = rightBorder;
        if (direction > 0)
        {
            border = rightBorder;
        }
        if (direction < 0)
        {
            border = leftBorder;
        }

        List<Location> newLocations = new();
        for(int i = Math.Abs(border) + 1; i <= Math.Abs(border) + amount; i++)
        {
            for(int j = topBorder; j >= 1; j--)
            {
                newLocations.Add(new Location(i * direction, j));
            }
        }
        return (newLocations, border + (amount * direction));
    }

    public void ExpandUp(int amount)
    {
        List<Location> newLocations = new();
        for(int i = topBorder + 1; i <= topBorder + amount; i++)
        {
            for(int j = leftBorder; j <= rightBorder; j++)
            {
                newLocations.Add(new Location(j, i));
            }
        }
        topBorder = topBorder + amount;
        MapExpanded?.Invoke(newLocations);
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