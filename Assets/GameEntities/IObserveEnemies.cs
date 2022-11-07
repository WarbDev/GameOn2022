using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserveEnemies 
{
    public void OnEnemyAdded(Enemy enemy);
    public void OnEnemyRemoved(Enemy enemy);
}
