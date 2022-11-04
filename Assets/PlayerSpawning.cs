using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] int X;
    [SerializeField] int Y;
    public void SpawnPlayers()
    {
        GameObject playerObject = Instantiate(playerPrefab);
        Player playerScript = playerObject.GetComponent<Player>();
        playerScript.Location = (X, Y);

        PlayerCollection.AddPlayer(playerScript);
        GameMap.PlayersDictionary.Add(playerScript.Location, playerScript);
    }
}
