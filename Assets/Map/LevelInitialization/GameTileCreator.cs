using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameTileCreator : MonoBehaviour
{
    [SerializeField] GridCreator gridCreator;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int scale;
    public event Action<MapTile> TileCreated;

    Queue<Location> locationsToAdd = new();
    bool isRunningRoutine = false;

    private void Awake()
    {
        gridCreator.AddedNode += AddGameTile;
    }

    void AddGameTile(Location location)
    {
        locationsToAdd.Enqueue(location);
        if (!isRunningRoutine)
        {
            isRunningRoutine = true;
            StartCoroutine(AddTiles());
        }
    }

    IEnumerator AddTiles()
    {
        while (locationsToAdd.Count > 0)
        {
            MakeGameTile(locationsToAdd.Dequeue());
            yield return new WaitForSeconds(.05f);
        }

        isRunningRoutine = false;
    }

    void MakeGameTile(Location location)
    {
        GameObject gridTile = Instantiate(tilePrefab);
        MapTile tileScript = gridTile.GetComponent<MapTile>();
        tileScript.transform.SetParent(gameObject.transform);
        tileScript.SetLocation(location);
        var targetPosition = Location2Position(location.X, location.Y);
        tileScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, 50f);
        var targetVector3overshoot = new Vector3(targetPosition.x, targetPosition.y, -0.4f);
        var targetVector3 = new Vector3(targetPosition.x, targetPosition.y, 0f);

        tileScript.gameObject.name = "Tile (" + location.X + ", " + location.Y + ")";

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3overshoot, 0.5f));
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3, 0.3f).SetEase(Ease.OutElastic));

        Entities.MapTileCollection.AddEntity(tileScript);
        TileCreated?.Invoke(tileScript);
    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
