using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Gravity")]
public class Move_Gravity : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    [SerializeField] int range;
    [SerializeField] int radius;
    [SerializeField] int pushStrength;
    ShapeWithRadius rangeShape = LocationUtility.LocationsInAllCardinalAndOrdinalDirections;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;

    List<PushLog> log;


    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;

        locator = new Locator_1ShapeAt1Range(rangeShape, effectShape, player.Location, range, radius);
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
        locations = effectShape(selected, radius);

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        log = new();
        List<PushRequest> requests = new();
        foreach (Enemy enemy in enemies)
        {
            Location direction = selected - enemy.Location;

            requests.Add(new PushRequest(enemy, direction, pushStrength));
        }

        log = Push.CalculatePushes(requests);
        Push.DoPushes(log);

        PlayGraphics(selected);

    }

    private void PlayGraphics(Location location)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Gravity animationManager = animation.GetComponent<A_Gravity>();

        animationManager.PlayAnimation(LocationUtility.LocationToVector3(location));

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<GravityAnimationProperties> obj)
    {
        foreach (PushLog lo in log)
        {
            lo.MoveLog.Entity.transform.position = LocationUtility.LocationToVector3(lo.MoveLog.Entity.Location);
        }

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
