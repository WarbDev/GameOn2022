using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileCreator : MonoBehaviour
{
    [SerializeField] int scale;
    [SerializeField] Sprite tileSprite;
    [SerializeField] ClickListener clickListener;
    public bool Finished = false;
    Dictionary<Location, MapTile> mapTileDictionary = new Dictionary<Location, MapTile>();

    public void MakeGameTiles()
    {
        
        for (int i = GameMap.TopLeft.X; i <= GameMap.TopRight.X; i++)
        {
            for (int j = 1; j <= GameMap.TopRight.Y; j++)
            {

                GameObject gridTile = new GameObject(); //Create the GameObject
                SpriteRenderer NewImage = gridTile.AddComponent<SpriteRenderer>(); //Add the Image Component script
                MapTile tileScript = gridTile.AddComponent<MapTile>();
                BoxCollider2D collider2D = gridTile.AddComponent<BoxCollider2D>();
                collider2D.isTrigger = true;
                ClickableEntity clickComponent = gridTile.AddComponent<ClickableEntity>();
                clickListener.clickables.Add(clickComponent);
                tileScript.Location = (i, j);
                NewImage.sprite = tileSprite; //Set the Sprite of the Image Component on the new GameObject
                gridTile.GetComponent<Transform>().position = Location2Position(i, j);
                gridTile.SetActive(true); //Activate the GameObject
                mapTileDictionary.Add((i, j) , tileScript);
            }
        }
        GameMap.MapTilesDictionary = mapTileDictionary;
        clickListener.enabled = true;
        Finished = true;
    }

    Vector2 Location2Position(int x, int y)
    {
        return new Vector2(x * scale, y * scale);
    }
}
