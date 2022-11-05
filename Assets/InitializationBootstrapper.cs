using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitializationBootstrapper : MonoBehaviour
{
    [SerializeField] GridCreator gridCreator;
    [SerializeField] GameTileCreator gameTileCreator;
    [SerializeField] PlayerSpawning playerSpawner;

    void Start()
    {
        AddPlayers();
    }

    void AddPlayers()
    {
        playerSpawner.SpawnPlayers();
    }
}
