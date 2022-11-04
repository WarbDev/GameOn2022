using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCreator : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] int scale;
    public bool Finished = false;

    public void MakeGameTiles()
    {
        
        for (int i = GameMap.TopLeft.X; i <= GameMap.TopRight.X; i++)
        {
            for (int j = 1; j <= GameMap.TopRight.Y; j++)
            {

                GameObject gridTile = Instantiate(tilePrefab);
                MapTile tileScript = gridTile.GetComponent<MapTile>();
                tileScript.Location = (i, j);
                gridTile.GetComponent<Transform>().position = Location2Position(i, j);
                gridTile.SetActive(true); //Activate the GameObject
                tileScript.gameObject.name = "Tile (" + i + ", " + j + ")";
                MapTileCollection.AddMapTile(tileScript);
            }
        }
        Finished = true;
    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
