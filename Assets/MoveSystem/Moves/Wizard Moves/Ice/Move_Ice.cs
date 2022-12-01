using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Ice")]
public class Move_Ice : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    ShapeWithRadius rangeShape = LocationUtility.LocationsInHorizonalLine;
    ShapeWithRadius effectShape = LocationUtility.LocationsAsRectangleWithLength;
    [SerializeField] int range;
    [SerializeField] int length;

    [SerializeField] GameObject iceTerrain;

    private List<Location> locations;
    private List<TerrainBase> terrainLog;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;
        locator = new Locator_1ShapeAt1Range(rangeShape, effectShape, player.Location, range, length);
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
        locations = effectShape(selected, length);

        List<MapTile> tiles = LocationUtility.GetTilesInPositions(locations);
        terrainLog = new();
        foreach (MapTile tile in tiles)
        {
            TerrainBase terrain = Entities.SpawnTerrain(tile.Location, iceTerrain);
            if (terrain)
            {
                terrainLog.Add(terrain);
            }
        }

        PlayGraphics();

    }

    private void PlayGraphics()
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Ice animationManager = animation.GetComponent<A_Ice>();

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animationManager.PlayAnimation(terrainLog);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<IceAnimationProperties> obj)
    {

        foreach (TerrainBase terrain in terrainLog)
        {
            terrain.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(terrain.SpriteRenderer));
        }

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
