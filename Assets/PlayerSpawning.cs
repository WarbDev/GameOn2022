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
        playerScript.GetComponent<Transform>().position = new Vector2(X, Y);
        playerScript.Location = (X, Y);

        EntityCollection.PlayerCollection.AddEntity(playerScript);
    }
}
