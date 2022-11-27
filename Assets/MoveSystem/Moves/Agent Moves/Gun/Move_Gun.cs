using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Gun")]
public class Move_Gun : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    ShapeWithRadius rangeShape = LocationUtility.LocationsInHorizonalLine;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;

    [SerializeField] float damage;
    [SerializeField] int range;




    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;
        locator = new Locator_1ShapeAt1Range(rangeShape, effectShape, player.Location, range, 0);
        locator.DeterminedLocations -= DoEffects;
        locator.DeterminedLocations += DoEffects;
        locator.StartLocate(this);
    }

    private void DoEffects(List<Location> locations)
    {
        locator.DeterminedLocations -= DoEffects;

        if (locations == null)
        {
            MoveCompleted?.Invoke(false);
            return;
        }

        Location selected = locations[0]; //locations[0] is the player-selected point

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        List<DamageLog> log = new();
        foreach (Enemy enemy in enemies)
        {
            log.Add(enemy.Damageable.DealDamage(new Damage(damage, player)));
        }

        PlayGraphics(selected, log);

    }

    private void PlayGraphics(Location location, List<DamageLog> log)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Gun animationManager = animation.GetComponent<A_Gun>();

        animationManager.PlayAnimation(LocationUtility.LocationToVector3(location), log);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<GunAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}

