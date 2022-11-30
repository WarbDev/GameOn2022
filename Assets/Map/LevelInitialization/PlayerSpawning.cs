using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] List<GameObject> players = new();

    public void SpawnPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i], new Location(0, i + 1));
        }
    }

    public void DespawnPlayers()
    {
        foreach(var player in players)
        {
            Entities.PlayerCollection.RemoveEntity(player.GetComponent<Player>());
            player.transform.position = new Vector3(200f, 200f, 200f);
        }
    }

    void SpawnPlayer(GameObject playerPrefab, Location location)
    {
        Player playerScript = playerPrefab.GetComponent<Player>();
        playerScript.SetLocation(location, true);

        Entities.PlayerCollection.AddEntity(playerScript);
    }
}
