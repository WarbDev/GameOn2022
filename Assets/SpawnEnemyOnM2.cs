using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyOnM2 : MonoBehaviour
{
    [SerializeField] ClickListener tileClickListener;
    [SerializeField] GameObject enemyPrefab;
    private void Start()
    {
        tileClickListener.EntityRightClicked += SpawnEnemy;
    }

    void SpawnEnemy(ClickableEntity tile)
    {
        var entityScript = tile.GetComponent<IGameEntity>();
        Location tileLocation = entityScript.Location;
        var enemy = Instantiate(enemyPrefab);
        enemy.GetComponent<Transform>().position = new Vector2(tileLocation.X, tileLocation.Y);
        EnemyCollection.AddEnemy(enemy.GetComponent<Enemy>());
    }
}