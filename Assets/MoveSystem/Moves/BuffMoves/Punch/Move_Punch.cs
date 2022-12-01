using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Punch")]
public class Move_Punch : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;

    [SerializeField] int radius;
    [SerializeField] float damage;
    [SerializeField] int pushDistance;
    [SerializeField] GameObject fireTerrain;
    public int Radius { get => radius; }
    public float Damage { get => damage; }
    ShapeWithRadius rangeShape = LocationUtility.LocationsInHorizonalLine;

    ShapeWithRadius effectExplodeShape = LocationUtility.LocationsInSquareRadius;

    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;
    Player player;

    List<PushLog> pushLog;
    List<DamageLog> damageLog;
    List<TerrainBase> terrainLog;

    public override event Action<bool> MoveCompleted;

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

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        if (enemies == null || enemies.Count == 0)
        {
            MoveCompleted?.Invoke(false);
            return;
        }

        pushLog = new();
        List<PushRequest> requests = new();
        foreach (Enemy enemy in enemies)
        {
            Location direction = enemy.Location - player.Location;

            requests.Add(new PushRequest(enemy, direction, pushDistance));
        }

        pushLog = Push.CalculatePushes(requests);
        Push.DoPushes(pushLog);


        //Exploding ball at end
        List<Location> location = effectExplodeShape(pushLog[0].MoveLog.End, radius);

        enemies = LocationUtility.GetEnemiesInPositions(location);
        damageLog = new();
        List<Action> scyth = new();
        foreach (Enemy enemy in enemies)
        {
            DamageableWithHealthComponent dam = enemy.GetComponent<DamageableWithHealthComponent>();

            damageLog.Add(dam.DealDamage(new Damage(damage, player), out Action triggerDeath));
            scyth.Add(triggerDeath);
        }


        locations = new();
        locations.Add(selected);
        List<MapTile> tiles = LocationUtility.GetTilesInPositions(locations);
        terrainLog = new();
        foreach (MapTile tile in tiles)
        {
            TerrainBase terrain = Entities.SpawnTerrain(tile.Location, fireTerrain);
            if (terrain)
            {
                terrainLog.Add(terrain);
            }
        }

        foreach (TerrainBase terrain in terrainLog)
        {
            terrain.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(terrain.SpriteRenderer));
        }

        PlayGraphics(selected, enemies[0], scyth);

    }

    private void PlayGraphics(Location location, Enemy enemy, List<Action> scyth)
    {

        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Punch animationManager = animation.GetComponent<A_Punch>();

        MapTile endPoint;
        LocationUtility.TryGetTile(location, out endPoint);

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));

        enemy.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(pushLog[0]));

        animationManager.PlayAnimation(endPoint.transform.position, damageLog, pushLog, enemy, scyth);

        //foreach (DamageLog damaged in damageLog)
        //{
        //    damaged.Target.Entity.GetComponent<AnimatableEntity>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        //}

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<PunchAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}