using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Compares against tags that obstructing entities contain.
// Could be refactored into an abstract class in order to compare things other than tags.
public class ObstructionCheckerComponent : EntityComponent, IObstructionChecker
{
    public IObstructionChecker ObstructionChecker { get => this; }
    [SerializeField] List<OBSTRUCTION_TAG> obstructingTags;

    public bool IsObstructedBy(IObstruct obstruction)
    {
        foreach(var tag in obstructingTags)
        {
            if (obstruction.ObstructionTags.Contains(tag))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsObstructedBy(IEnumerable<IObstruct> obstructions)
    {
        foreach (var obstruction in obstructions)
        {
            if (IsObstructedBy(obstruction))
                return true;
        }
        return false;
    }

    public bool IsObstructedBy(Location location)
    {
        return IsObstructedBy(LocationUtility.GetObstructionsAtPosition(location));
    }
}

public interface IObstructionChecker
{
    public IObstructionChecker ObstructionChecker { get; }
    public bool IsObstructedBy(IObstruct obstruction)
    {
        return ObstructionChecker.IsObstructedBy(obstruction);
    }

    public bool IsObstructedBy(IEnumerable<IObstruct> obstructions)
    {
        return ObstructionChecker.IsObstructedBy(obstructions);
    }

    public bool IsObstructedBy(Location location)
    {
        return ObstructionChecker.IsObstructedBy(LocationUtility.GetObstructionsAtPosition(location));
    }

    public bool IsObstructedBy(IEnumerable<Location> locations)
    {
        foreach(var location in locations)
        {
            if (ObstructionChecker.IsObstructedBy(location))
            {
                return true;
            }
        }
        return false;
    }
}

