using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstructionChecker))]
public class Pushable : EntityComponent, IObstructionChecker
{
    [SerializeField] ObstructionChecker obstructionChecker;

    public GameEntity Entity { get => GameEntity; }
    public ObstructionChecker ObstructionChecker { get => obstructionChecker; }

    public bool IsObstructedBy(IObstructingEntity obstruction)
    {
        return ObstructionChecker.IsObstructedBy(obstruction);
    }

    public bool IsObstructedBy(IEnumerable<IObstructingEntity> obstructions)
    {
        return ((IObstructionChecker)ObstructionChecker).IsObstructedBy(obstructions);
    }
}

public interface IHavePushable
{
    public Pushable Pushing { get; }
}

public interface IPushableEntity : IHaveGameEntity, IHavePushable
{

}