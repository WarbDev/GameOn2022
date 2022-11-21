using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Punch")]
public class Move_Punch : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;

    [SerializeField] int range;
    [SerializeField] int radius;
    [SerializeField] float damage;
    [SerializeField] GameObject Projectile;
    public int Range { get => range; }
    public int Radius { get => radius; }
    public float Damage { get => damage; }
    ShapeWithRadius rangeShape = LocationUtility.LocationsInSquareRadius;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;
    Player player;


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

        Location selected = locations[0]; //locations[0] is the player-selected point
        locations = effectShape(selected, radius);

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        List<DamageLog> log = new();
        foreach (Enemy enemy in enemies)
        {
            log.Add(enemy.Damageable.DealDamage(new DamageDetails(damage, player)));
        }

        PlayGraphics(selected, log);
        
    }

    private void PlayGraphics(Location location, List<DamageLog> log)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Punch animationManager = animation.GetComponent<A_Punch>();

        MapTile endPoint;
        LocationUtility.TryGetTile(location, out endPoint);

        animationManager.PlayAnimation(endPoint.transform.position, log);
    }
}