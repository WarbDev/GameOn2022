using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Compares against tags that obstructing entities contain.
// Could be refactored into an abstract class in order to compare things other than tags.
public class ObstructionChecker : EntityComponent, IObstructionChecker
{
    [SerializeField] List<OBSTRUCTION_TAG> obstructingTags;

    public bool IsObstructedBy(IObstructingEntity obstruction)
    {
        foreach(var tag in obstructingTags)
        {
            if (obstruction.Obstruct.ObstructionTags.Contains(tag))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsObstructedBy(IEnumerable<IObstructingEntity> obstructions)
    {
        foreach (var obstruction in obstructions)
        {
            if (IsObstructedBy(obstruction))
                return true;
        }
        return false;
    }
}

public interface IObstructionChecker
{
    public bool IsObstructedBy(IObstructingEntity obstruction);
    public bool IsObstructedBy(IEnumerable<IObstructingEntity> obstructions);
}

