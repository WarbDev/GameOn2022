using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        gridTile.GetComponent<Transform>().position = Location2Position(location.X, location.Y);
        
        gridTile.SetActive(LocationUtility.IsOnMap(location)); // Activate the GameObject if on the map
        tileScript.gameObject.name = "Tile (" + location.X + ", " + location.Y + ")";
        MapTileCollection.AddMapTile(tileScript);
    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
