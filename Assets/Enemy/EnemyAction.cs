using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class EnemyAction : EntityComponent
{
    [SerializeField] AnimatableEntity animatableEntity;
    public abstract event Action<EnemyAction> ActionFinished;

    public abstract void DoEnemyAction();
}
