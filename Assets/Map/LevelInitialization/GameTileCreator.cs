using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Pool;
using System.Linq;

public class GameTileCreator : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject darkTileRow;
    [SerializeField] int scale;

    [SerializeField] float baseSpeedMult = 1f;
    [SerializeField] bool speedy;

    [Tooltip("How many times faster the tiles are created if speedy is checked.")]
    [SerializeField] float speedyMultiplier;

    public event Action<MapTile> TileCreated;

    
    bool isRunningRoutine = false;
    ObjectPool<MapTile> pool;
    ObjectPool<GameObject> darkTileRowPool;
    List<GameObject> activeRows = new();

    public float SpeedyMultiplier { get { if (speedy) return speedyMultiplier * baseSpeedMult; else return 1f * baseSpeedMult; } }

    private void Awake()
    {
        pool = new ObjectPool<MapTile>(() => { return Instantiate(tilePrefab, new Vector3(500, 500), new Quaternion()).GetComponent<MapTile>(); },
            tile => { tile.gameObject.SetActive(true); },
            tile => { tile.gameObject.SetActive(false); },
            tile => { Destroy(tile); },
            true, 100, 500);

        darkTileRowPool = new ObjectPool<GameObject>(() => { return Instantiate(darkTileRow, new Vector3(500, 500), new Quaternion()); },
            tile => { activeRows.Add(tile); tile.SetActive(true); },
            tile => { tile.SetActive(false); },
            tile => { activeRows.Remove(tile); Destroy(tile); },
            true, 12, 50);
    }

    private void OnDestroy()
    {
        pool.Clear();
    }

    public Func<bool> RaisePlayerLine(int height)
    {
        var playerColumn = MakeGameTiles(LocationUtility.GetColumn(0));
        StartCoroutine(AscendMapTiles(playerColumn, height));

        return (() => false);
    }

    public Func<bool> LowerPlayerLine(int height)
    {
        StartCoroutine(DescendMapTiles(GetMapTilesInColumn(0), height));
        return (() => false);
    }

    public Func<bool> UnbuildMapTiles()
    {
        isRunningRoutine = true;
        StartCoroutine(UnbuildBattlefieldRoutine(GameMap.LeftBorder, GameMap.RightBorder, GameMap.TopBorder));
        return (() => isRunningRoutine);
    }

    public Func<bool> BuildMapTiles((int, int, int) lrh)
    {
        isRunningRoutine = true;
        StartCoroutine(BuildBattlefieldRoutine(lrh.Item1, lrh.Item2, lrh.Item3));

        return (() => isRunningRoutine);
    }



    IEnumerator BuildBattlefieldRoutine(int left, int right, int height)
    {
        List<List<MapTile>> columns = new();

        int furthest = Math.Max(Math.Abs(left), right);
        for (int i = 1; i <= furthest; i++)
        {
            if (right >= i)
            {
                columns.Add(MakeGameTiles(LocationUtility.GetColumn(i)));
            }

            if (left >= -i)
            {
                columns.Add(MakeGameTiles(LocationUtility.GetColumn(-i)));
            }
        }

        foreach(var column in columns)
        {
            StartCoroutine(AscendMapTiles(column, height));
            yield return new WaitForSeconds(0.2f / SpeedyMultiplier);
        }

        for(int i = 0; i < height; i++)
        {
            var row = darkTileRowPool.Get();
            var row2 = darkTileRowPool.Get();
            AttachRows(i + 1, row, row2);
        }

        yield return new WaitForSeconds(0.5f);


        isRunningRoutine = false;
    }

    IEnumerator UnbuildBattlefieldRoutine(int left, int right, int height)
    {
        List<List<MapTile>> columns = new();

        int furthest = Math.Max(Math.Abs(left), right);
        left = Math.Abs(left);
        for (int i = 1; i <= furthest; i++)
        {
            if (right >= i)
            {
                columns.Add(GetMapTilesInColumn(i));
            }

            if (left >= -i)
            {
                columns.Add(GetMapTilesInColumn(-i));
            }
        }



        foreach (var column in columns)
        {
            StartCoroutine(DescendMapTiles(column, height));
            yield return new WaitForSeconds(0.2f / SpeedyMultiplier);
        }
        
        for(int i = 0; i < activeRows.Count; i++)
        {
            Sequence sequence = DOTween.Sequence();
            activeRows[i].transform.DOLocalMoveZ(1000, 1f).SetEase(Ease.OutExpo);
            yield return new WaitForSeconds(0.05f);
            darkTileRowPool.Release(activeRows[i]);
        }
        yield return new WaitForSeconds(1f);
        darkTileRowPool.Dispose();

        isRunningRoutine = false;
    }

    List<MapTile> MakeGameTiles(List<Location> locations)
    {
        List<MapTile> list = new();
        foreach (var l in locations)
        {
            list.Add(MakeGameTile(l));
        }
        return list;
    }

    List<MapTile> GetMapTilesInColumn(int x)
    {
        List<MapTile> tiles = new List<MapTile>();
        var col = LocationUtility.GetColumn(x);
        foreach (var loc in col)
        {
            LocationUtility.TryGetTile(loc, out MapTile tile);
            tiles.Add(tile);
        }
        return tiles;
    }

    MapTile MakeGameTile(Location location)
    {
        MapTile tileScript = pool.Get();
        tileScript.transform.SetParent(gameObject.transform);
        tileScript.SetLocation(location);
        tileScript.gameObject.name = "Tile (" + location.X + ", " + location.Y + ")";

        Entities.MapTileCollection.AddEntity(tileScript);
        if (LocationUtility.TryGetTerrain(location, out TerrainBase terrain))
        {
            terrain.SetLocation(location, true);
        }
        TileCreated?.Invoke(tileScript);
        return tileScript;
    }

    void RemoveGameTile(Location location)
    {
        LocationUtility.TryGetTile(location, out MapTile tile);
        Entities.MapTileCollection.RemoveEntity(tile);
        pool.Release(tile);
    }
    
    IEnumerator AscendMapTiles(List<MapTile> tiles, int height)
    {
        IEnumerable<MapTile> ordered = tiles.OrderBy(tile => Math.Abs((height / 2) - tile.Location.Y));

        foreach (var tile in ordered)
        {
            AscendMapTile(tile, tile.Location);
            yield return new WaitForSeconds(0.05f / SpeedyMultiplier);
        }
    }

    void AttachRows(int height, GameObject row1, GameObject row2)
    {
        int xLeftPosition = GameMap.LeftBorder;
        int xRightPosition = GameMap.RightBorder;

        int xPosition = GameMap.LeftBorder;

        GameObject[] gameObjects = { row1, row2 };
        foreach (var row in gameObjects)
        {
            int targetXPosition; 
            if (xPosition > 0)
            {
                targetXPosition = xPosition + 1;
            }
            else
            {
                targetXPosition = xPosition - 16;
            }




            Vector3 targetPosition = new(targetXPosition, height, 0f);
            row.transform.position = targetPosition + new Vector3(60 * Mathf.Sign(xPosition), 0, 0);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(row.transform.DOMove(targetPosition, 0.8f).SetEase(Ease.InOutSine));
            xPosition = GameMap.RightBorder;
        }
    }

    IEnumerator DescendMapTiles(List<MapTile> tiles, int height)
    {
        IEnumerable<MapTile> ordered = tiles.OrderByDescending(tile => Math.Abs((height / 2) - tile.Location.Y));

        foreach (var tile in ordered)
        {
            DescendMapTile(tile, tile.Location);
            yield return new WaitForSeconds(0.05f / SpeedyMultiplier);
        }
    }



    void AscendMapTile(MapTile tileScript, Location location)
    {
        var targetPosition = Location2Position(location.X, location.Y);
        tileScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, 50f);
        var targetVector3overshoot = new Vector3(targetPosition.x, targetPosition.y, -0.2f);
        var targetVector3 = new Vector3(targetPosition.x, targetPosition.y, 0f);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3overshoot, 0.8f).SetEase(Ease.InOutSine));
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3, 0.7f).SetEase(Ease.InSine));
    }

    void DescendMapTile(MapTile tileScript, Location location)
    {
        var targetPosition = Location2Position(location.X, location.Y);
        tileScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
        var targetVector3overshoot = new Vector3(targetPosition.x, targetPosition.y, -0.2f);
        var targetVector3 = new Vector3(targetPosition.x, targetPosition.y, -50f);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3overshoot, 0.8f).SetEase(Ease.InOutSine));
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3, 0.3f).SetEase(Ease.InSine)).OnComplete(onComplete);

        void onComplete()
        {
            RemoveGameTile(location);
        }
    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
