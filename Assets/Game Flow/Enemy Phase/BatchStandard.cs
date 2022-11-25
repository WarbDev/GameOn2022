using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a container that, when asked of, can produce where an enemy should be spawned, and what type that enemy should be.
/// This container only does so when it is possible to do so.
/// What determines when it is possible is the set of rules that implementations (children) of the class contain.
/// The implementations also determine which enemies should spawn and where.
/// </summary>
public abstract class BatchBase : ScriptableObject
{

    public abstract void ResetBatch();

    public abstract Dictionary<Location, ENEMY_TYPE> RunBatch(out bool isFinished);
}

/// <summary>
/// The one-size-fits-all batch implementation. Covers a lot of bases in allowing you to be specific with how you want your enemies to spawn.
/// </summary>
[CreateAssetMenu(fileName = "Standard Batch", menuName = "ScriptableObjects/StandardBatch")]
public class BatchStandard : BatchBase
{
    [Tooltip("Enemies to populate in the batch. Ordered by what should be prioritized to spawn further at the top of the column.")]
    [SerializeField] List<ENEMY_TYPE> enemiesInBatch;

    [Tooltip("If checked, the batch will consider empty spaces to be a mandatory addition to a column. This can be used to ensure that enemies" +
        " in the batch are spaced out from other enemies in the same batch, but does not prevent enemies from spawning right next to " +
        "enemies from a previous batch.")]
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

    [Tooltip("If checked, the batch will not spawn unless the entire column can support the batch. Cannot be unchecked if isRigid is true." +
        " If unchecked, the batch will populate as many available spaces as possible, and try to spawn the rest in later columns.")]
    [SerializeField] bool mustFitSingleColumn;

    List<ENEMY_TYPE> enemiesLeftInBatch;

    private void OnValidate()
    {
        if (isRigid)
        {
            mustFitSingleColumn = true;
        }
    }

    // I'm sorry.
    public override Dictionary<Location, ENEMY_TYPE> RunBatch(out bool isFinished)
    {
        if (enemiesLeftInBatch.Count == 0)
        {
            ResetBatch();
        }

        isFinished = false;
        Dictionary<Location, ENEMY_TYPE> enemiesToPopulate = new();
        var primaryColumn = LocationUtility.GetEndColumn(side);
        var secondaryColumn = LocationUtility.GetEndColumn(side.Other());

        // If there are enough spaces
        //if ((populatableLocations = GetPopulatableLocationsInColumn(primaryColumn, isRigid)).Count > MinimumNeededSpacesToFill(enemiesLeftInBatch))
        //{
            enemiesToPopulate = BuildPopulationsFromColumn(enemiesToPopulate, out isFinished, primaryColumn);
        //}
        if (!isFinished && !enforceSide)
        {
            enemiesToPopulate = BuildPopulationsFromColumn(enemiesToPopulate, out isFinished, secondaryColumn);
        }



        return enemiesToPopulate;
    }

    Dictionary<Location, ENEMY_TYPE> BuildPopulationsFromColumn(Dictionary<Location, ENEMY_TYPE> currentPopDic, out bool isFinished, List<Location> column)
    {
        column = GetPopulatableLocationsInColumn(column, isRigid);
        currentPopDic = currentPopDic.Concat(PopulateLocations(column, enemiesLeftInBatch, out List<ENEMY_TYPE> remainingEnemies)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        isFinished = remainingEnemies.Count == 0;
        enemiesLeftInBatch = remainingEnemies;
        return currentPopDic;
    }


    List<Location> GetPopulatableLocationsInColumn(List<Location> column, bool rigidity)
    {
        if (!rigidity)
            return LocationUtility.GetLocationsWithNoObstruction(column);
        else
            return LocationUtility.GetLongestUnobstructedLocationSequence(column);
    }

    int MinimumNeededSpacesToFill(List<ENEMY_TYPE> batchEnemies)
    {
        if (isRigid)
            return SpacesNeededToCompletelyFill(batchEnemies);
        else
        {
            return 1;
        }
    }

    int SpacesNeededToCompletelyFill(List<ENEMY_TYPE> batchEnemies)
    {
        int needed = 0;
        foreach (var enemy in batchEnemies)
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

    Dictionary<Location, ENEMY_TYPE> PopulateLocations(List<Location> populatableLocations, List<ENEMY_TYPE> enemiesToPopulate, out List<ENEMY_TYPE> remainingEnemies)
    {
        remainingEnemies = new();
        Dictionary<Location, ENEMY_TYPE> populationDictionary = new();

        if (mustFitSingleColumn && SpacesNeededToCompletelyFill(enemiesToPopulate) > populatableLocations.Count)
        {
            remainingEnemies = enemiesToPopulate;
            return populationDictionary;
        }

        if (enemiesToPopulate.Count > populatableLocations.Count && CanBatchBeSqueezed(enemiesToPopulate, populatableLocations.Count))
            enemiesToPopulate = SqueezeToFit(enemiesToPopulate, populatableLocations.Count);

        for (int i = 0; i < enemiesToPopulate.Count; i++)
        {
            if (i >= populatableLocations.Count)
            {
                remainingEnemies.Add(enemiesToPopulate[i]);
            }
            else if (enemiesToPopulate[i] != ENEMY_TYPE.NONE)
                populationDictionary.Add(populatableLocations[i], enemiesToPopulate[i]);
        }
        return populationDictionary;
    }

    bool CanBatchBeSqueezed(List<ENEMY_TYPE> enemies, int comparison)
    {
        if (enforceEmptySpaces)
        {
            return false;
        }
        return enemies.Where(enemy => enemy != ENEMY_TYPE.NONE).Count() <= comparison;
    }

    int EnemyTypesThatAreNone(List<ENEMY_TYPE> enemyTypes)
    {
        return enemyTypes.Where(enemy => enemy == ENEMY_TYPE.NONE).Count();
    }

    /// <summary>
    /// Takes a list of ENEMY_TYPE and removes those of type NONE so that it can fit.
    /// Will throw an exception if it is incapable of squeezing.
    /// </summary>
    List<ENEMY_TYPE> SqueezeToFit(List<ENEMY_TYPE> enemies, int space)
    {
        List<ENEMY_TYPE> squeezedEnemies = new();
        int noneTypes = EnemyTypesThatAreNone(enemies);
        int spacesThatNeedClearing = enemies.Count - space;
        List<bool> shouldClear = new();

        // Generates a list so that NONE tags can be randomly eliminated. 
        for (int i = 0; i < spacesThatNeedClearing; i++)
        {
            shouldClear.Add(false);
        }

        // Randomly goes through the list until enough spaces are marked as needing clearing.
        // Horribly unoptimized with larger lists. Could literally take forever.
        int spacesMarkedShouldClear = 0;
        while (spacesMarkedShouldClear < spacesThatNeedClearing)
        {
            var randomIndex = Random.Range(0, shouldClear.Count);
            if (!shouldClear[randomIndex])
            {
                shouldClear[randomIndex] = true;
                spacesMarkedShouldClear++;
            }
        }


        int clearedSpaces = 0;
        foreach (ENEMY_TYPE enemy in enemies)
        {
            if (!(clearedSpaces >= shouldClear.Count) && enemy == ENEMY_TYPE.NONE && shouldClear[clearedSpaces])
            {
                clearedSpaces++;
            }
            else
            {
                squeezedEnemies.Add(enemy);
            }
        }
        return squeezedEnemies;
    }

    public override void ResetBatch()
    {
        enemiesLeftInBatch = new List<ENEMY_TYPE>(enemiesInBatch);
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