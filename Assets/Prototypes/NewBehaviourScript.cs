using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prototype", menuName = "ScriptableObjects/Prototypething")]
/* This is a fireball
 * that deals damage in an area
 * That has a chance to deal more damage.
 * 
 * It sets tiles on fire (Changes their sprites)
 * 
 * It makes the player do an animation
 * has an animation of moving towards the selected point
 * and does an explosion animation there
 * 
 * and has variable range.
 */

public class NewBehaviourScript : ScriptableObject
{
    public int damage;
    public int damageSecondary;
    public float SecondaryChance = .5f;
    public GameObject projectile;

    public Shape rangeShape = Shape.Square;
    public int rangeRadius = 5;

    public Shape fireShape = Shape.Square;
    public int fireRadius = 2;
}
