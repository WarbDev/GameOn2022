using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstructionCheckerComponent))]
public class PushableComponent : EntityComponent, IObstructionChecker, IPushable
{
    public IPushable Pushable { get => this; }
    [SerializeField] ObstructionCheckerComponent obstructionChecker;

    public GameEntity Entity { get => GameEntity; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }

    public bool IsObstructedBy(IObstruct obstruction)
    {
        return ObstructionChecker.IsObstructedBy(obstruction);
    }

    public bool IsObstructedBy(IEnumerable<IObstruct> obstructions)
    {
        return ((IObstructionChecker)ObstructionChecker).IsObstructedBy(obstructions);
    }
}


public interface IPushable : IHaveGameEntity, IObstructionChecker
{
    public IPushable Pushable { get; }
}