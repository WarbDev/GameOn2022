using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovement : EntityComponent
{
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] AnimatableEntity animatableEntity;
    [SerializeField] Enemy stun;

    Location Location { get => GameEntity.Location; }
    public event Action<EnemyMovement> MovementFinished;

    // Calculates movement, changes position on map.
    // Invokes MovementFinished if there was no change in position.
    // Else, plays an animation.
    // Once the animation is finished, MovementFinished is invoked.
    public void DoTurnMovement()
    {
        Location startLocation = Location;
        Location targetLocation = DetermineLocationToMoveTo();

        if (Location != targetLocation)
        {
            GameMap.MoveEnemy(GameEntity, targetLocation);

            var animation = MakeAnimation(startLocation, targetLocation);
            animation.AnimationFinished += MoveEndTick;
        }

        else
        {
            MoveEndTick();
        }
    }

    void MoveEndTick(EntityAnimation<JumpAnimationProperties> animation)
    {
        animation.AnimationFinished -= MoveEndTick;
        MoveEndTick();
    }

    void MoveEndTick()
    {
        var hasTerrain = LocationUtility.TryGetTerrain(Location, out TerrainBase terrain);
        Func<bool> isFinished = ()=>true;
        if (hasTerrain)
        {
            isFinished = terrain.OnEntityMoveOver(gameEntity);
        }

        StartCoroutine(MoveEndRoutine());
        IEnumerator MoveEndRoutine()
        {
            while (!isFinished())
            {
                yield return null;
            }
            MovementFinished?.Invoke(this);
        }
    }

    EntityAnimation<JumpAnimationProperties> MakeAnimation(Location start, Location end)
    {
        return animatableEntity.PlayAnimation(ANIMATION_ID.ENTITY_JUMP,
                new JumpAnimationProperties(start, end));
    }

    Location DetermineLocationToMoveTo()
    {
        if (stun.StunDuration > 0)
        {
            return Location;
        }

        Location desiredLocation = (Location.X + LocationUtility.DirectionTowardsCenter(Location), Location.Y);
        if (obstructionChecker.IsObstructedBy(desiredLocation))
        {
            return Location;
        }

        return desiredLocation;
    }
}
