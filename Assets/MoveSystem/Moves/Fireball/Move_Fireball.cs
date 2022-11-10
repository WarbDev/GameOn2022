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
    public int Range { get => range; }
    public int Radius { get => radius; }
    public float Damage { get => damage; }
    ShapeWithRadius rangeShape = LocationUtility.LocationsInSquareRadius;
    ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;
    Player player;


    

    private List<Location> locations;

    public override void DoMove(Player player)
    {
        this.player = player;
        if (locator != null)
        {
            throw new Exception("You cannot do a move while another move is running"); 
        }
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
        List<MapTile> tiles = LocationUtility.GetTilesInPositions(locations);
        List<DamageLog> log = new();
        foreach (Enemy enemy in enemies)
        {
            log.Add(enemy.DealDamage(new DamageDetails(damage, player)));
        }
        foreach (MapTile tile in tiles)
        {
            //
            //SET TILES ON FIRE
            //TERRAIN MODIFIER
            //
        }

        
        GameObject Fireball = Instantiate(Projectile);
        Fireball.transform.position = player.transform.position;
        A_Fireball animation = Fireball.GetComponent<A_Fireball>();

        MapTile endPoint;
        LocationUtility.TryGetTile(selected, out endPoint);
        
        animation.PlayAnimation(endPoint.transform.position);

        locator = null;
    }
}