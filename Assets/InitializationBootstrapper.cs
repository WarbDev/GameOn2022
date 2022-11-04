using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitializationBootstrapper : MonoBehaviour
{
    [SerializeField] GridCreator gridCreator;
    [SerializeField] GameTileCreator gameTileCreator;
    [SerializeField] PlayerSpawning playerSpawner;
    private IEnumerator gridCreationCoroutine;
    private IEnumerator tileCreationCoroutine;

    void Start()
    {
        gridCreationCoroutine = DoGridCreationProcess();
        tileCreationCoroutine = DoTileCreationProcess();
        StartCoroutine(DoGridCreationProcess());
        AddPlayers();
    }


    IEnumerator DoGridCreationProcess()
    {
        gridCreator.CreateGrid();
        while (true)
        {
            if (gridCreator.Finished)
            {
                StartCoroutine(tileCreationCoroutine);
                StopCoroutine(gridCreationCoroutine);
            }
            yield return null;
        }
    }

    IEnumerator DoTileCreationProcess()
    {
        gameTileCreator.MakeGameTiles();
        while (true)
        {
            if (gameTileCreator.Finished)
            {
                StopCoroutine(tileCreationCoroutine);
            }
            yield return null;
        }
    }

    void AddPlayers()
    {
        playerSpawner.SpawnPlayers();
    }
}
