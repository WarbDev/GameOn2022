using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a container that, when asked of, can produce where an enemy should be spawned, and what type that enemy should be.
/// This container only does so when it is possible to do so.
/// What determines when it is possible is the set of rules that implementations (children) of the class contain.
/// The implementations also determine which enemies should spawn and where.
/// </summary>
public abstract class BatchBase : ScriptableObject
{
    public abstract bool TryRunBatch(out Dictionary<Location, ENEMY_TYPE> batch);
}

/// <summary>
/// The one-size-fits-all batch implementation. Covers a lot of bases in allowing you to be specific with how you want your enemies to spawn.
/// We probably won't need other implementations. We probably shouldn't make other implementations.
/// </summary>
[CreateAssetMenu(fileName = "Standard Batch", menuName = "ScriptableObjects/StandardBatch")]
public class BatchStandard : BatchBase
{
    [Tooltip("Enemies to populate in the batch. Ordered by what should be prioritized to spawn further at the top of the column." +
        " Lists that are of a size larger than the height of a column will result in the batch potentially never running.")]
    [SerializeField] List<ENEMY_TYPE> enemiesInBatch;

    [Tooltip("If checked, the batch will consider empty spaces to be a mandatory addition to a column. This can be used to ensure that enemies" +
        " in the batch are spaced out from other enemies in the same batch, but does not prevent enemies from spawning right next to " +
        "enemies from the previous batch.")]
    [SerializeField] bool enforceEmptySpaces;

    [Tooltip("Which side should the batch try to spawn on first?")]
    [SerializeField] MAP_SIDE side;

    [Tooltip("If checked, the batch will only try to run on the side given. If unchecked, the batch will try to run on the other side if " +
        "the first side given is not capable.")]
    [SerializeField] bool enforceSide;

    [Tooltip("If checked, all elements of the batch must be able to be added onto the column in an unbroken line. " +
        "For example, if 1 represents an occupied space, and 0 is unoccupied, if a column is 10010, a batch marked as rigid that has 3 occupying elements " +
        "cannot populate that column, as the column has only 2 consecutive available spaces.")]
    [SerializeField] bool isRigid;

    // I'm sorry.
    public override bool TryRunBatch(out Dictionary<Location, ENEMY_TYPE> dictionary)
    {
        var column = new List<Location>();
        var primaryColumn = LocationUtility.GetEndColumn(side);

        if (TryGetPopulatableLocationsInColumn(primaryColumn, isRigid, out column))
        {
            dictionary = PopulateLocations(column, enemiesInBatch);
            return true;
        }

        if (!enforceSide)
        {
            var secondaryColumn = LocationUtility.GetEndColumn(side.Other());
            if (TryGetPopulatableLocationsInColumn(secondaryColumn, isRigid, out column))
            {
                dictionary = PopulateLocations(column, enemiesInBatch);
                return true;
            }
        }

        dictionary = null;
        return false;
    }

    bool TryGetPopulatableLocationsInColumn(List<Location> column, bool rigidity, out List<Location> populatableLocations)
    {
        if (!rigidity)
        {
            populatableLocations = LocationUtility.GetLocationsWithNoObstruction(column);

        }
        else
        {
            populatableLocations = LocationUtility.GetLongestUnobstructedLocationSequence(column);
        }
        return populatableLocations.Count >= GetSpacesNeeded();
    }

    int GetSpacesNeeded()
    {
        int needed = 0;
        foreach(var enemy in enemiesInBatch)
        {
            if (enemy == ENEMY_TYPE.NONE)
            {
                if (enforceEmptySpaces)
                    needed++;
            }
            else
            {
                needed++;
            }
        }
        return needed;
    }

    Dictionary<Location, ENEMY_TYPE> PopulateLocations(List<Location> locationsToPopulate, List<ENEMY_TYPE> enemiesToPopulate)
    {
        Dictionary<Location, ENEMY_TYPE> populationDictionary = new();
        for (int i = 0; i < enemiesToPopulate.Count; i++)
        {
            if (enemiesToPopulate[i] != ENEMY_TYPE.NONE)
                populationDictionary.Add(locationsToPopulate[i], enemiesToPopulate[i]);
        }
        return populationDictionary;
    }
}

/// <summary>
/// Represents either the left or right side of the map.
/// </summary>
public enum MAP_SIDE
{
    LEFT,
    RIGHT
}

public static class MAP_SIDE_Extensions
{
    /// <summary>
    /// Returns the opposite side.
    /// </summary>
    public static MAP_SIDE Other(this MAP_SIDE side)
    {
        if (side == MAP_SIDE.LEFT)
        {
            return MAP_SIDE.RIGHT;
        }
        else return MAP_SIDE.LEFT;
    }
}