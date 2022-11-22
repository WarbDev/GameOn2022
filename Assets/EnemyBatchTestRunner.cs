using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBatchTestRunner : MonoBehaviour
{
    [SerializeField] BatchBase batch;
    [SerializeField] EnemyTypeDictionary PrefabDictionary;

    List<ENEMY_TYPE> backLog = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DoNextBatch();
        }
    }

    void DoNextBatch()
    {
        //if (backLog.Count > 0)
        //{
        //    backLog = DoBatch(backLog);
        //}
        //backLog.AddRange(DoBatch(batch));
    }

    //List<ENEMY_TYPE> DoBatch(IEnumerable<ENEMY_TYPE> enumerable)
    //{
    //    var accumulatedBackLog = new List<ENEMY_TYPE>();
    //    foreach(var enemy in enumerable)
    //    {
    //        var hasAvailableLocation = TryNextAvailableLocation(out Location location);
    //        if (hasAvailableLocation)
    //        {
    //            Entities.SpawnEnemy(location, PrefabDictionary.PrefabDictionary[enemy]);
    //        }
    //        else
    //        {
    //            accumulatedBackLog.Add(enemy);
    //        }
    //    }
    //    return accumulatedBackLog;
    //}

    //bool TryNextAvailableLocation(out Location location)
    //{
    //    foreach(var column in LocationUtility.GetEndColumns())
    //    {
    //        foreach(var loc in column)
    //        {
    //            if (!LocationUtility.TryGetEnemy(loc, out Enemy e))
    //            {
    //                location = loc;
    //                return true;
    //            }
    //        }
    //    }
    //    location = new(0, 0);
    //    return false;
    //}

}
