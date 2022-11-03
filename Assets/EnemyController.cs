using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    System.Random rnd = new System.Random();
    //int month = rnd.Next(1, 13);  // creates a number between 1 and 12
    public GameObject Enemy; //The enemy Prefab
    public static List<GameObject> enemies = new List<GameObject>();
    public static event Action<GameObject[]> eNextTurn;

    public void NextTurn() {

        for (int i = 0; i < enemies.Count; i++) {
            GameObject e = enemies[i];
            Transform t = e.GetComponent<Transform>();
            Vector2 ePosition = t.position;
            int x = (int)ePosition.x;
            int y = (int)ePosition.y;
            GameObject go = Grid.Map[x - 1, y];
            if (go == null && 2 <= x) {
                t.position = new Vector2 (ePosition.x - 1, ePosition.y);
                Grid.UpdateMap(e, x, y);
            } else if(go != null && go.GetComponent<Player>() != null){
                go.GetComponent<Player>().Damage(1);
            }
        }

        GameObject[] enemyArray = new GameObject[5]; 
        for(int i = 0; i<enemyArray.Length; i++) {
            if (rnd.Next(2) == 1 && Grid.Map[9,i] == null) {
                GameObject e = Instantiate(Enemy);
                enemyArray[i] = e;
                enemies.Add(e);
            } else {
                enemyArray[i] = null;
            }
        }

        eNextTurn?.Invoke(enemyArray);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
