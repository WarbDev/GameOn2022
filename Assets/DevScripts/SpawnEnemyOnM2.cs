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
        Location location = tile.gameObject.GetComponent<GameEntity>().Location;
        if (LocationUtility.TryGetEnemy(location, out Enemy e))
        {
            return;
        }
        Entities.SpawnEnemy(location, enemyPrefab);
    }
}
