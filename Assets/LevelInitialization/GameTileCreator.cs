using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameTileCreator : MonoBehaviour
{
    [SerializeField] GridCreator gridCreator;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int scale;


    private void Awake()
    {
        gridCreator.AddedNode += MakeGameTile;
    }

    void MakeGameTiles(IEnumerable<Location> locations)
    {
        foreach(var location in locations)
        {
            MakeGameTile(location);
        }
    }

    void MakeGameTile(Location location)
    {
        GameObject gridTile = Instantiate(tilePrefab);
        MapTile tileScript = gridTile.GetComponent<MapTile>();
        tileScript.Location = (location.X, location.Y);
        var targetPosition = Location2Position(location.X, location.Y);
        tileScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, 50f);
        var targetVector3overshoot = new Vector3(targetPosition.x, targetPosition.y, -0.3f);
        var targetVector3 = new Vector3(targetPosition.x, targetPosition.y, 0f);


        gridTile.SetActive(LocationUtility.IsOnMap(location)); // Activate the GameObject if on the map
        tileScript.gameObject.name = "Tile (" + location.X + ", " + location.Y + ")";

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3overshoot, 0.5f));
        mySequence.Append(tileScript.gameObject.transform.DOMove(targetVector3, 0.1f));

        Entities.MapTileCollection.AddEntity(tileScript);

    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
