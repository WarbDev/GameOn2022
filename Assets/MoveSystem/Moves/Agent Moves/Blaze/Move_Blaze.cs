using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Blaze")]
public class Move_Blaze : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    [SerializeField] GameObject fireTerrain;
    Player player;

    [SerializeField] int range;
    [SerializeField] int damage;
    ShapeWithRadius rangeShape = LocationUtility.LocationsInCone;

    private List<TerrainBase> terrainLog;

    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;

        List<Location> area = rangeShape(player.Location, range);
        locator = new Locator_StaticArea(area);
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

        List<Location> area;
        List<Location> burnArea;
        if (locations[0].X > 0)
        {
            area = rangeShape(player.Location, range);
            burnArea = LocationUtility.LocationsInEmptyCone(player.Location, range);

        }
        else
        {
            area = LocationUtility.FlipLocations(rangeShape(player.Location, range));
            burnArea = LocationUtility.FlipLocations(LocationUtility.LocationsInEmptyCone(player.Location, range));
        }

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(area);
        List<DamageLog> log = new();
        foreach (Enemy enemy in enemies)
        {
            log.Add(enemy.Damageable.DealDamage(new Damage(damage, player)));
        }


        List<MapTile> tiles = LocationUtility.GetTilesInPositions(burnArea);
        terrainLog = new();
        foreach (MapTile tile in tiles)
        {
            TerrainBase terrain = Entities.SpawnTerrain(tile.Location, fireTerrain);
            if (terrain)
            {
                terrainLog.Add(terrain);
            }
        }

        PlayGraphics(locations[0], log);

    }

    private void PlayGraphics(Location location, List<DamageLog> log)
    {
        GameObject animation = Instantiate(animatorObject);
        //animation.transform.position = player.transform.position;

        A_Blaze animationManager = animation.GetComponent<A_Blaze>();

        animationManager.PlayAnimation();

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<BlazeAnimationProperties> obj)
    {
        foreach (TerrainBase terrain in terrainLog)
        {
            terrain.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(terrain.GetComponent<SpriteRenderer>()));
        }

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
