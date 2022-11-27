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
    [SerializeField] int scale;

    [SerializeField] bool speedy;

    [Tooltip("How many times faster the tiles are created if speedy is checked.")]
    [SerializeField] float speedyMultiplier;

    public event Action<MapTile> TileCreated;

    
    bool isRunningRoutine = false;
    ObjectPool<MapTile> pool;

    public float SpeedyMultiplier { get { if (speedy) return speedyMultiplier; else return 1f; } }

    private void Awake()
    {
        pool = new ObjectPool<MapTile>(() => { return Instantiate(tilePrefab, new Vector3(500, 500), new Quaternion()).GetComponent<MapTile>(); },
            tile => { tile.gameObject.SetActive(true); },
            tile => { tile.gameObject.SetActive(false); },
            tile => { Destroy(tile); },
            true, 100, 500);
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

    public Func<bool> LowerPlayerLine()
    {
        return (() => false);
    }
    public Func<bool> UnbuildMapTiles()
    {
        return (() => false);
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

    MapTile MakeGameTile(Location location)
    {
        MapTile tileScript = pool.Get();
        tileScript.transform.SetParent(gameObject.transform);
        tileScript.SetLocation(location);
        tileScript.gameObject.name = "Tile (" + location.X + ", " + location.Y + ")";

        Entities.MapTileCollection.AddEntity(tileScript);
        TileCreated?.Invoke(tileScript);
        return tileScript;
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

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
