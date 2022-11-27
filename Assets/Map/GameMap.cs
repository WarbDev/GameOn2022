using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameMap : MonoBehaviour
{
    public static event Action<int> TopBorderExpanded;

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
    static Dictionary<Location, GameEntity> mapTilesDictionary = new();
    static Dictionary<Location, GameEntity> playersDictionary = new();
    static Dictionary<Location, GameEntity> enemiesDictionary = new();
    static Dictionary<Location, GameEntity> terrainDictionary = new();
    static Dictionary<EntityType, Dictionary<Location, GameEntity>> typeDictionary = new();
    static List<Dictionary<Location, GameEntity>> allEntityDictionaries = new();

    public static Dictionary<Location, GameEntity> MapTilesDictionary
    {
        get => mapTilesDictionary;
        private set => mapTilesDictionary = value; 
    }

    public static Dictionary<Location, GameEntity> PlayersDictionary
    {
        get => playersDictionary;
        private set => playersDictionary = value;
    }

    public static Dictionary <Location, GameEntity> EnemiesDictionary
    {
        get => enemiesDictionary;
        private set => enemiesDictionary = value;
    }

    public static Dictionary<Location, GameEntity> TerrainDictionary
    {
        get => terrainDictionary;
        private set => terrainDictionary = value;
    }

    public static List<Dictionary<Location, GameEntity>> AllEntityDictionaries
    { 
        get => allEntityDictionaries;
        private set => allEntityDictionaries = value; 
    }

    #endregion


    void Awake()
    {
        DOTween.SetTweensCapacity(500, 312);
        typeDictionary.Clear();

        mapTilesDictionary = new();
        playersDictionary = new();
        enemiesDictionary = new();
        terrainDictionary = new();

        typeDictionary.Add(EntityType.ENEMY, EnemiesDictionary);
        typeDictionary.Add(EntityType.PLAYER, PlayersDictionary);
        typeDictionary.Add(EntityType.MAPTILE, MapTilesDictionary);
        typeDictionary.Add(EntityType.TERRAIN, TerrainDictionary);

        allEntityDictionaries.Add(EnemiesDictionary);
        allEntityDictionaries.Add(PlayersDictionary);
        allEntityDictionaries.Add(MapTilesDictionary);
        allEntityDictionaries.Add(TerrainDictionary);

        BottomBorder = 1;

        Entities.PlayerCollection.EntityAdded += OnEntityAdded;
        Entities.PlayerCollection.EntityRemoved += OnEntityRemoved;

        Entities.EnemyCollection.EntityAdded += OnEntityAdded;
        Entities.EnemyCollection.EntityRemoved += OnEntityRemoved;

        Entities.MapTileCollection.EntityAdded += OnEntityAdded;
        Entities.MapTileCollection.EntityRemoved += OnEntityRemoved;

        Entities.TerrainCollection.EntityAdded += OnEntityAdded;
        Entities.TerrainCollection.EntityRemoved += OnEntityRemoved;
    }

    private void OnDestroy()
    {
        Entities.PlayerCollection.EntityAdded -= OnEntityAdded;
        Entities.PlayerCollection.EntityRemoved -= OnEntityRemoved;

        Entities.EnemyCollection.EntityAdded -= OnEntityAdded;
        Entities.EnemyCollection.EntityRemoved -= OnEntityRemoved;

        Entities.MapTileCollection.EntityAdded -= OnEntityAdded;
        Entities.MapTileCollection.EntityRemoved -= OnEntityRemoved;

        Entities.TerrainCollection.EntityAdded -= OnEntityAdded;
        Entities.TerrainCollection.EntityRemoved -= OnEntityRemoved;
    }

    public static void SetBorders(int l, int r, int h)
    {
        leftBorder = -Math.Abs(l);
        rightBorder = r;
        topBorder = h;
    }

    public static void SetBorders((int, int, int) lrh)
    {
        SetBorders(lrh.Item1, lrh.Item2, lrh.Item3);
        TopBorderExpanded?.Invoke(lrh.Item3);
    }

    public static void MovePlayer(GameEntity player, Location to)
    {
        if (PlayersDictionary.ContainsKey(to))
        {
            throw new Exception("Trying to move to a player to an occupied position! Do SwapPlayers instead!");
        }

        Location oldLocation = player.Location;
        PlayersDictionary.Remove(oldLocation);
        PlayersDictionary.Add(to, player);

        player.GetComponent<Player>().SetLocation(to);
        player.transform.localPosition = Vector3.zero;
    }

    public static void SwapPlayers(GameEntity player1, GameEntity player2)
    {
        Location location1 = player1.Location;
        Location location2 = player2.Location;

        PlayersDictionary[location1] = player2;
        PlayersDictionary[location2] = player1;

        Location tempLocation = new Location(player1.Location.X, player1.Location.Y);

        player1.GetComponent<Player>().SetLocation(new Location(player2.Location.X, player2.Location.Y));
        player2.GetComponent<Player>().SetLocation(tempLocation);
    }

    public static void MoveEnemy(GameEntity enemy, Location to)
    {
        Location oldLocation = enemy.Location;
        EnemiesDictionary.Remove(oldLocation);
        enemy.GetComponent<Enemy>().SetLocation(to);
        EnemiesDictionary.Add(to, enemy);
    }

    //the indexes of the list of enemies and the list of locations must match
    //and must match each other 
    //i.e. enemies[3] will move to togo[3]
    public static void MoveEnemies(List<GameEntity> enemies, List<Location> togo)
    {
        if (enemies.Count != togo.Count)
        {
            throw new Exception("Count of enemies to move and locations to go to is not the same!");
        }

        foreach (GameEntity enemy in enemies)
        {
            Location oldLocation = enemy.Location;
            EnemiesDictionary.Remove(oldLocation);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Enemy>().SetLocation(togo[i]);
            EnemiesDictionary.Add(togo[i], enemies[i]);
        }
    }

    void OnEntityAdded(GameEntity entity)
    {
        typeDictionary[entity.EntityType].Add(entity.Location, entity);
    }
    void OnEntityRemoved(GameEntity entity)
    {
        typeDictionary[entity.EntityType].Remove(entity.Location);
    }
}