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
    public int Range { get => range; }
    public int Radius { get => radius; }
    public float Damage { get => damage; set => damage = value; }

    [SerializeField] float damage;

    private List<Location> locations;

    public void DoMove()
    {
        if (locator != null)
        {
            throw new Exception("You cannot do a move while another move is running"); 
        }
        locator = new Locator_SquareAtRange(range, radius);  //=================================================================================
        locator.determinedLocations += CalculateEffects;
        locator.StartLocate(this);
    }

    private void CalculateEffects(List<Location> locations)
    {
        locator.determinedLocations -= CalculateEffects;
    }
}
