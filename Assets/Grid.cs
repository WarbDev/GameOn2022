using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public List<GameObject> players = new List<GameObject>();

    public static GameObject[,] Map = new GameObject[10,5];

    public int scale; //scale from location to position

    public Sprite TileSprite; //List of Sprites added from the Editor to be created as GameObjects at runtime
    //public GameObject ParentPanel; //Parent Panel you want the new Images to be children of

    public Vector2 location2Position(int x, int y){
        return new Vector2(x*scale, y*scale);
    }

    //takes an old gameobject and it's old coordinates, or a new gameobject with new coordinates
    public static void UpdateMap(GameObject g, int x, int y) {
        if (Map[x,y] = g) {
            Map[x, y] = null;
            Vector2 pos = g.GetComponent<Transform>().position;
            int x2 = (int)pos.x;
            int y2 = (int)pos.y;
            Map[x2, y2] = g;
        } else{
            Map[x, y] = g;
        }
    }

        // Start is called before the first frame update
    void Start(){
        for (int i = 0; i < Map.GetLength(0); i++) {
            for (int j = 0; j < Map.GetLength(1); j++) {
             
                GameObject gridTile = new GameObject(); //Create the GameObject
                SpriteRenderer NewImage = gridTile.AddComponent<SpriteRenderer>(); //Add the Image Component script
                NewImage.sprite = TileSprite; //Set the Sprite of the Image Component on the new GameObject
                                              //NewObj.GetComponent<RectTransform>().SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
                gridTile.GetComponent<Transform>().position = location2Position(i,j);
                gridTile.SetActive(true); //Activate the GameObject
            }
        }

        for(int i = 0; i < players.Count; i++){
            players[i].GetComponent<Transform>().position = location2Position(0, i);
            Map[0, i] = players[i];
        }

        EnemyController.eNextTurn += NextTurn;
    }

    void NextTurn(GameObject[] enemies) {
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i]) {
                enemies[i].GetComponent<Transform>().position = location2Position(9, i);
                Map[9, i] = enemies[i];
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}