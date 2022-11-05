using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnemyTurn
{
    List<List<Location>> GetColumns()
    {
        int top = GameMap.TopBorder;
        int right = GameMap.RightBorder;
        int left = GameMap.LeftBorder;

        List<List<Location>> columns = new();
        List<List<Location>> rightColumns = new();
        List<List<Location>> leftColumns = new();

        for (int i = 1; i <= right; i++)
        {
            List<Location> rightColumn = new();
            for (int j = top; j > 0; j--)
            {
                rightColumn.Add((j, i));
            }
            rightColumns.Add(rightColumn);
        }

        for (int i = -1; i <= left; i--)
        {
            List<Location> leftColumn = new();
            for (int j = top; j > 0; j--)
            {
                leftColumn.Add((j, i));
            }
            leftColumns.Add(leftColumn);
        }

        int mostColumns = rightColumns.Count;
        if (rightColumns.Count < leftColumns.Count)
        {
            mostColumns = leftColumns.Count;
        }

        for (int i = 0; i < mostColumns; i++)
        {
            if (i < rightColumns.Count)
            {
                columns.Add(rightColumns[i]);
            }

            if (i < leftColumns.Count)
            {
                columns.Add(leftColumns[i]);
            }
            i++;
        }

        return columns;
    }

    // For each column, get commands to move
    
}

public class EnemyMoveRequest
{
    Enemy enemy;
    Location moveLocation;

    public Enemy Enemy { get => enemy; }
    public Location MoveLocation { get => moveLocation; }

    public EnemyMoveRequest(Enemy enemy, Location endLocation)
    {
        this.enemy = enemy;
        moveLocation = endLocation;
    }
}