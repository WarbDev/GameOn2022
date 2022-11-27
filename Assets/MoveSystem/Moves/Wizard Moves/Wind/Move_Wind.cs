using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Moves/Wind")]
public class Move_Wind : Move
{
    private ILocate locator;
    [SerializeField] int range;
    [SerializeField] int radius;
    [SerializeField] int pushStrength;
    [SerializeField] GameObject AnimationObject;
    ShapeWithRadius rangeShape = LocationUtility.LocationsInSquareRadius;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;
    Player player;

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
        }

        Location selected = locations[0]; //locations[0] is the player-selected point
        locations = effectShape(selected, radius);

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        List<PushLog> log = new();
        List<PushRequest> requests = new();
        foreach (Enemy enemy in enemies)
        {
            Location direction = enemy.Location - selected;

            requests.Add(new PushRequest(enemy, direction, pushStrength));
        }

        log = Push.CalculatePushes(requests);
        Push.DoPushes(log);

        PlayGraphics(selected, log);

    }

    private void PlayGraphics(Location location, List<PushLog> log)
    {
        GameObject Wind = Instantiate(AnimationObject);
        A_Wind animation = Wind.GetComponent<A_Wind>();

        MapTile endPoint;
        LocationUtility.TryGetTile(location, out endPoint);
        Wind.transform.position = endPoint.transform.position;

        foreach (PushLog lo in log)
        {
            lo.MoveLog.Entity.transform.position = LocationUtility.LocationToVector3(lo.MoveLog.Entity.Location);
        }

        animation.PlayAnimation();

        animation.windAnimation.AnimationFinished -= MoveDone;
        animation.windAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<WindAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
