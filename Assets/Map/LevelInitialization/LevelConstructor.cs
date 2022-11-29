using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] GameTileCreator tileCreator;
    [SerializeField] PlayerSpawning playerSpawning;
    [SerializeField] WallScript wall;

    public event Action Finished;

    public void Run(Wave newWave)
    {
        StartCoroutine(Construct(newWave));
    }

    IEnumerator Construct(Wave currentWave)
    {
        GameMap.SetBorders(currentWave.WaveMapSize);

        // Raise the player line first, passing in the height of the new map.
        Func<bool> isBusy = tileCreator.RaisePlayerLine(currentWave.WaveMapSize.Item3);
        yield return new WaitForSeconds(0.5f);
        while (isBusy())
        {
            yield return null;
        }


        isBusy = tileCreator.BuildMapTiles(currentWave.WaveMapSize);
        while (isBusy())
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        


        bool wallFinished = false;
        wall.Finished += onWallFinish;
        wall.RaiseWall();
        void onWallFinish()
        {
            wallFinished = true;
        }

        while (!wallFinished)
        {
            yield return null;
        }
        wall.Finished -= onWallFinish;
        playerSpawning.SpawnPlayers();

        StartCoroutine(Deconstruct(currentWave));

        yield return new WaitForSeconds(10f);
        Finished?.Invoke();
    }

    IEnumerator Deconstruct(Wave currentWave)
    {
        bool wallFinished = false;
        wall.Finished += onWallFinish;
        wall.LowerWall();
        void onWallFinish()
        {
            wallFinished = true;
        }
        while (!wallFinished)
        {
            yield return null;
        }


        // Raise the player line first, passing in the height of the new map.
        Func<bool> isBusy = tileCreator.LowerPlayerLine(GameMap.TopBorder);
        yield return new WaitForSeconds(0.5f);
        while (isBusy())
        {
            yield return null;
        }

        isBusy = tileCreator.UnbuildMapTiles();
        while (isBusy())
        {
            yield return null;
        }
        
        Finished?.Invoke();
    }
}