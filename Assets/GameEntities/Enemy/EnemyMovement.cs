using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovement : EntityComponent
{
    [SerializeField] ObstructionChecker obstructionChecker;
    [SerializeField] AnimatableEntity animatableEntity;


    Location Location { get => GameEntity.Location; }
    public event Action<EnemyMovement> EnemyMovementAnimationFinished;

    // Calculates movement, changes position on map, and runs animation.
    // Once the animation is finished, EnemyMovementAnimationFinished is invoked.
    public void DoTurnMovement()
    {
        Location targetLocation = (Location.X + DirectionTowardsPlayers(), Location.Y);
        if (!obstructionChecker.IsObstructedBy(LocationUtility.GetObstructionsAtPosition(targetLocation)))
        {
            Location newLocation = (Location.X + DirectionTowardsPlayers(), Location.Y);
            MoveLog log = new MoveLog(GameEntity, Location, newLocation);
            GameMap.MoveEnemy(GameEntity, newLocation);

            var animation = animatableEntity.PlayAnimation(ANIMATION_ID.ENTITY_JUMP,
                    new JumpAnimationProperties(LocationUtility.LocationToVector3(log.Start), LocationUtility.LocationToVector3(log.End)));

            animation.AnimationFinished += OnAnimationFinished;
        }

        void OnAnimationFinished<T>(EntityAnimation<T> animation) where T : IAnimationProperties
        {
            animation.AnimationFinished -= OnAnimationFinished;
            EnemyMovementAnimationFinished?.Invoke(this);
        }
        

        int DirectionTowardsPlayers()
        {
            return System.Math.Sign(Location.X * -1);
        }
    }
}
