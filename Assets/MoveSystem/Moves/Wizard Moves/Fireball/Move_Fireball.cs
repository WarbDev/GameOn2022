using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Fireball")]

public class Move_Fireball : Move, IDamage
{
    private ILocate locator;
    [SerializeField] int range;
    [SerializeField] int radius;
    [SerializeField] float damage;
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject fireTerrain;
    [SerializeField] int turnsTillExtinguish;
    public int Range { get => range; }
    public int Radius { get => radius; }
    public float Damage { get => damage; }
    ShapeWithRadius rangeShape = LocationUtility.LocationsInDiagonalLines;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;
    Player player;

    private List<TerrainBase> terrainLog;




    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        fireTerrain.GetComponent<FireTerrain>().TurnsUntilExtinguish = turnsTillExtinguish;
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

        List<IDamageable> damageables = LocationUtility.GetDamageablesAtPositions(locations);
        List<MapTile> tiles = LocationUtility.GetTilesInPositions(locations);
        List<DamageLog> log = new();
        terrainLog = new();
        foreach (IDamageable dam in damageables)
        {
            log.Add(dam.DealDamage(new Damage(damage, player)));
        }
        foreach (MapTile tile in tiles)
        {
            TerrainBase terrain = Entities.SpawnTerrain(tile.Location, fireTerrain);
            if (terrain)
            {
                terrainLog.Add(terrain);
            }
        }

        

        PlayGraphics(selected, log);
        
    }

    private void PlayGraphics(Location location, List<DamageLog> log)
    {
        GameObject Fireball = Instantiate(Projectile);
        Fireball.transform.position = player.transform.position;
        A_Fireball animation = Fireball.GetComponent<A_Fireball>();

        MapTile endPoint;
        LocationUtility.TryGetTile(location, out endPoint);

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animation.PlayAnimation(endPoint.transform.position, log);
        animation.boomFireball.AnimationFinished -= MoveDone;
        animation.boomFireball.AnimationFinished += MoveDone;

    }

    private void MoveDone(EntityAnimation<BFireballAnimationProperties> obj)
    {
        foreach (TerrainBase terrain in terrainLog)
        {
            terrain.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(terrain.SpriteRenderer));
        }

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}