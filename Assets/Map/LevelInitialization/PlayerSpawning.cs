using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] List<GameObject> players = new();
    [SerializeField] GameTileCreator tileCreator;

    private void Start()
    {
        StartCoroutine(PlayerSpawningRoutine());
        
    }

    IEnumerator PlayerSpawningRoutine()
    {
        int i = 1;
        tileCreator.TileCreated += CheckToAddTile;
      
        while(i <= players.Count)
        {
            yield return null;
        }

        void CheckToAddTile(MapTile tile)
        {
            if (tile.Location.X == 0)
            {
                SpawnPlayer(players[i - 1], new Location(0, i));
                i++;
            }
        }

        tileCreator.TileCreated -= CheckToAddTile;
    }

    public void SpawnPlayer(GameObject playerPrefab, Location location)
    {
        Player playerScript = playerPrefab.GetComponent<Player>();
        playerScript.SetLocation(location, true);

        Entities.PlayerCollection.AddEntity(playerScript);
    }
}
