using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(ObstructionCheckerComponent))]
public abstract class EnemyActionSelectionComponent : EntityComponent
{
    [SerializeField] protected EnemyStunned stunnedAction;
    [SerializeField] protected ObstructionCheckerComponent obstructionChecker;
    [SerializeField] protected Enemy enemy;
    public abstract event Action<EnemyAction> ActionFinished;
    [SerializeField] protected IStunnable Stun { get => enemy; }
    protected IObstructionChecker ObstructionChecker { get => obstructionChecker; }

    public abstract void MakeAction();


}
