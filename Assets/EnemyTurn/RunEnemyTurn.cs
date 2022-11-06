using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnemyTurn : MonoBehaviour
{
    void DoEnemyTurn()
    {
        var columns = GetColumns();
    }


    List<MoveRequest> GenerateMoveRequestsInPositions(List<Location> positions)
    {
        List<MoveRequest> moveRequests = new();

        List<Enemy> enemiesInPositions = LocationUtility.GetEnemiesInPositions(positions);
        foreach (var enemy in enemiesInPositions)
        {
            MoveRequest request = enemy.MakeMoveRequest();
            if (request != null)
            {
                moveRequests.Add(request);
            }
        }

        return moveRequests;
    }

    List<List<Location>> GetColumns()
    {
        List<List<Location>> leftColumns = GetLeftColumns();
        List<List<Location>> rightColumns = GetRightColumns();
        List<List<Location>> columns = Interweave(ref rightColumns, ref leftColumns);
        return columns;
    }
    List<List<Location>> GetRightColumns()
    {
        List<List<Location>> columnList = new();
        for (int i = 1; i <= GameMap.RightBorder; i++)
        {
            List<Location> rightColumn = new();
            for (int j = GameMap.TopBorder; j > 0; j--)
            {
                rightColumn.Add((j, i));
            }
            columnList.Add(rightColumn);
        }
        return columnList;
    }
    List<List<Location>> GetLeftColumns()
    {
        List<List<Location>> columns = new();
        for (int i = -1; i <= GameMap.LeftBorder; i--)
        {
            List<Location> leftColumn = new();
            for (int j = GameMap.TopBorder; j > 0; j--)
            {
                leftColumn.Add((j, i));
            }
            columns.Add(leftColumn);
        }
        return columns;
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
            i++;
        }
        return interweavedList;
    }
}

public class MoveRequest
{
    IGameEntity entity;
    Location moveLocation;

    public IGameEntity Entity { get => entity; }
    public Location MoveLocation { get => moveLocation; }

    public MoveRequest(IGameEntity entity, Location endLocation)
    {
        this.entity = entity;
        moveLocation = endLocation;
    }
}