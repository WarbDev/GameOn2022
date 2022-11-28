using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Barricade")]
public class Move_Barricade : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    [SerializeField] GameObject barricadeTerrain;

    ShapeWithRadius rangeShape = LocationUtility.LocationsInHorizonalLine;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;

    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    List<TerrainBase> terrainLog = new();

    public override void DoMove(Player player)
    {
        this.player = player;
        locator = new Locator_1ShapeAt1Range(rangeShape, effectShape, player.Location, 1, 0);
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

        TerrainBase terrain = Entities.SpawnTerrain(selected, barricadeTerrain);
        if (terrain)
        {
            terrainLog.Add(terrain);
        }

        PlayGraphics(selected);

    }

    private void PlayGraphics(Location selected)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = LocationUtility.LocationToVector3(selected);

        A_Barricade animationManager = animation.GetComponent<A_Barricade>();

        animationManager.PlayAnimation();

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<BarricadeAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
