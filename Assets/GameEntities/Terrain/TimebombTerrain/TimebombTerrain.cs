using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimebombTerrain : TerrainBase, IPushable
{
    Location location;
    [SerializeField] PushableComponent pushable;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;


    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.TERRAIN; }
    public IPushable Pushable { get => pushable; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }


    [SerializeField] int radius;
    [SerializeField] int stunDuration;
    [SerializeField] SpriteAnimation explodeAnimation;
    [SerializeField] AudioClip explodeClip;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;

    public int TurnsUntilBoom;

    private EntityAnimation<SpriteAnimationProperties> animationPlaying;

    public override Func<bool> OnEntityMoveOver(GameEntity entity)
    {
        return ()=> true; // p
    }

    public override void OnRoundTick()
    {
        TurnsUntilBoom--;
        if (TurnsUntilBoom <= 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        //animationPlaying = Animatable.PlayAnimation(ANIMATION_ID.MOVE_PAYLOAD, new SpriteAnimationProperties(gameObject.GetComponent<SpriteRenderer>()));
        explodeAnimation.Play(new SpriteAnimationProperties(gameObject.GetComponent<SpriteRenderer>()));
        GlobalAudioSource.Instance.Play(explodeClip);
        List<Location> areaOfEffect = effectShape(Location, radius);
        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(areaOfEffect);

        foreach (Enemy enemy in enemies)
        {
            enemy.AddStun(stunDuration);
        }

        explodeAnimation.AnimationFinished += end;
        //animationPlaying.AnimationFinished += end;
    }

    private void end(EntityAnimation<SpriteAnimationProperties> entityAnimation)
    {
        Entities.RemoveEntity(this);
        Destroy(gameObject);
    }
}
