using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Fireball")]

public class Move_Fireball : ScriptableObject, IMove, IDamage
{
    private ILocate locator;
    [SerializeField] int range;
    [SerializeField] int radius;
    //[SerializeField] enum LocatorKey;
    public int Range { get => range; }
    public int Radius { get => radius; }
    public float Damage { get => damage; set => damage = value; }
    //private ILocateShapeWithRadius shape = new Locator_Square(radius);
    [SerializeField] ShapeWithRadius rangeShape = LocationUtility.LocationsInSquareRadius;
    [SerializeField] ShapeWithRadius effectShape = LocationUtility.LocationsInSquareRadius;


    [SerializeField] float damage;

    private List<Location> locations;

    public void DoMove(Player player)
    {
        if (locator != null)
        {
            throw new Exception("You cannot do a move while another move is running"); 
        }
        locator = new Locator_1ShapeAt1Range(rangeShape, effectShape, player.Location, range, radius);
        locator.DeterminedLocations += CalculateEffects;
        locator.StartLocate(this);
    }

    private void CalculateEffects(List<Location> locations)
    {
        locator.DeterminedLocations -= CalculateEffects;
    }
}