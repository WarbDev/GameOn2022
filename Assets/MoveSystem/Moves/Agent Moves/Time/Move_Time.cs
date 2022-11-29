using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Time")]
public class Move_Time : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;
    [SerializeField] int range;
    [SerializeField] GameObject timeBombTerrain;
    ShapeWithRadius rangeShape = LocationUtility.LocationsInSquareRadius;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;

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

        TimebombTerrain bomb = (TimebombTerrain) Entities.SpawnTerrain(locations[0], timeBombTerrain);

        if (bomb == null)
        {
            MoveCompleted?.Invoke(false);
            return;
        }

        PlayGraphics(bomb, LocationUtility.LocationToVector3(locations[0]));

    }

    private void PlayGraphics(TimebombTerrain bomb, Vector3 endPoint)
    {
        GameObject animation = Instantiate(animatorObject);
        bomb.transform.position = player.transform.position;

        A_Time animationManager = animation.GetComponent<A_Time>();

        animationManager.PlayAnimation(bomb, endPoint);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<TimeAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}

