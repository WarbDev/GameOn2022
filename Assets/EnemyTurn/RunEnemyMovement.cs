using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RunEnemyMovement : MonoBehaviour
{
    public event Action<List<MoveLog>> CalculatedEnemiesMovement;
    public event Action Finished;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DoEnemyTurn();
        }
    }

    void DoEnemyTurn()
    {
        List<MoveLog> allMoveLogs = new();
        List<List<MoveLog>> columnMoveLogs = new();

        foreach (var column in AllColumns())
        {
            List<MoveLog> columnLogs = MoveEnemiesInPositions(column);
            allMoveLogs.AddRange(columnLogs);
            columnMoveLogs.Add(columnLogs);
        }
        CalculatedEnemiesMovement?.Invoke(allMoveLogs);
        Finished?.Invoke();
    }


    List<MoveLog> MoveEnemiesInPositions(List<Location> positions)
    {
        List<MoveLog> moveRequests = new();

        List<Enemy> enemiesInPositions = LocationUtility.GetEnemiesInPositions(positions);
        foreach (var enemy in enemiesInPositions)
        {
            MoveLog log = enemy.DoTurnMovement();
            if (log != null)
            {
                moveRequests.Add(log);
            }
        }

        return moveRequests;
    }

    // Generates all columns,
    // first generates all right columns from center->left
    // then generates all right columns from center->right
    IEnumerable<List<Location>> AllColumns()
    {
        foreach(var column in LeftColumns())
        {
            yield return column;
        }
        foreach(var column in RightColumns())
        {
            yield return column;
        }
    }

    IEnumerable<List<Location>> RightColumns()
    {
        for (int i = 1; i <= GameMap.RightBorder; i++)
        {
            List<Location> rightColumn = new List<Location>(GameMap.TopBorder);
            for (int j = GameMap.TopBorder; j > 0; j--)
            {
                rightColumn.Add((i, j));
            }
            yield return rightColumn;
        }
    }

    IEnumerable<List<Location>> LeftColumns()
    {
        for (int i = -1; i >= GameMap.LeftBorder; i--)
        {
            List<Location> leftColumn = new List<Location>(GameMap.TopBorder); ;
            for (int j = GameMap.TopBorder; j > 0; j--)
            {
                leftColumn.Add((i, j));
            }
            yield return leftColumn;
        }
    }

 

    // Takes two lists and places the elements of each list one after another as many times as possible.
    // If one list has fewer elements than the other, then the remaining elements of the longer list will
    // be simply all added at the end.
    List<T> Interweave<T>(ref List<T> list1, ref List<T> list2) where T : IList
    {
        List<T> interweavedList = new();
        int highestCount = list1.Count;
        if (list1.Count < list2.Count)
        {
            highestCount = list2.Count;
        }

        for (int i = 0; i < highestCount; i++)
        {
            if (i < list1.Count)
            {
                interweavedList.Add(list1[i]);
            }

            if (i < list2.Count)
            {
                interweavedList.Add(list2[i]);
            }
        }
        return interweavedList;
    }
}

public class MoveLog
{
    IGameEntity entity;
    Location start;
    Location end;

    public IGameEntity Entity { get => entity; }
    public Location Start { get => start; }
    public Location End { get => end; }

    public MoveLog(IGameEntity entity, Location start, Location end)
    {
        this.entity = entity;
        this.start = start;
        this.end = end;
    }
}