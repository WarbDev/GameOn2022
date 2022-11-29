using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class EnemyAction : EntityComponent
{
    [SerializeField] protected ObstructionCheckerComponent obstructionCheckerComponent;
    [SerializeField] protected AnimatableEntity animatableEntity;
    [SerializeField] protected EntityAnimation actionAnimation;
    public IObstructionChecker ObstructionChecker { get => obstructionCheckerComponent; }
    public IAnimatable Animatable { get => animatableEntity; }

    public abstract event Action<EnemyAction> ActionFinished;

    public abstract void DoEnemyAction();

    public static IDamageable NonAlliedObstructionInFront(Location location, IObstructionChecker ObstructionChecker)
    {
        Location frontLocation = LocationUtility.NextClosestToCenter(location);
        bool hasEnemy = LocationUtility.HasEnemy(location);
        IDamageable damageable = LocationUtility.GetDamageableInLocation(frontLocation);
        List<IObstruct> obstructions = LocationUtility.GetObstructionsAtPosition(frontLocation);

        bool isObstructedByNonAlly = ObstructionChecker.IsObstructedBy(obstructions) && !obstructions.Any(obstruction => obstruction.Entity.EntityType == EntityType.ENEMY);
        if (isObstructedByNonAlly)
        {
            return damageable;
        }
        else return null;
    }

    public static bool IsPlayerLineInHorizontalRange(int minDistance, int maxDistance, Location location)
    {
        int absX = Math.Abs(location.X);
        return absX >= minDistance && absX <= maxDistance;
    }

    public static bool OnSameXAsPlayer(Location location)
    {
        return LocationUtility.HasPlayer((0, location.Y));
    }

    public static List<Location> HorizontalLocationsInRange(int minDistance, int maxDistance, int dir, Location location)
    {
        List<Location> result = new List<Location>();
        for (int i = minDistance; i <= maxDistance; i++)
        {
            Location newLoc = new Location(location.X + i * dir, location.Y);
            result.Add(newLoc);
        }
        return result;
    }
}
