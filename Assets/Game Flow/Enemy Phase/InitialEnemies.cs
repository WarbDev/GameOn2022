using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Initial Enemies", menuName = "ScriptableObjects/Initial Enemies")]
public class InitialEnemies : ScriptableObject
{

    [Tooltip("Int represents the column number. The list represents each enemy in the column.")]
    [SerializeField] GenericDictionary<int, List<ENEMY_TYPE>> enemies;

    [SerializeField] int columnHeight;
    [SerializeField] List<int> columns;

    [InspectorButton("Create")]
    public bool Initialize;

    void Create()
    {
        enemies.Clear();
        foreach(var x in columns)
        {
            List<ENEMY_TYPE> enemiesInColumn = new();
            for(int i = 1; i <= columnHeight; i++)
            {
                enemiesInColumn.Add(ENEMY_TYPE.NONE);
            }
            enemies.Add(x, enemiesInColumn);
        }
    }

    public Dictionary<Location, ENEMY_TYPE> GetInitialEnemies()
    {
        Dictionary<Location, ENEMY_TYPE> dic = new();
        foreach(var kvp in enemies)
        {
            for(int i = 0; i < kvp.Value.Count; i++)
            {
                if (kvp.Value[i] != ENEMY_TYPE.NONE)
                dic.Add((kvp.Key, i + 1), kvp.Value[i]);
            }
        }
        return dic;
    }
}
