using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity, IHaveHealth, IDamageable, IAnimatable, IObstruct
{
    Location location;
    public override Location Location { get => location; protected set => location = value; }
    public override EntityType EntityType { get => EntityType.PLAYER; }

    [SerializeField] Health health;
    public SpriteRenderer adjustedSpriteRenderer;
    public Health Health { get => health; }
    public bool IsDead { get => Damageable.IsDead; }


    [SerializeField] DamageableWithHealthComponent damageableHealth;
    [SerializeField] ObstructionCheckerComponent obstructionChecker;
    [SerializeField] Obstruction obstruction;
    [SerializeField] AnimatableEntity playerAnimations;
    [SerializeField] PlayerTurnComponent turnComponent;
    [SerializeField] FaceCamera faceCamera;
    [SerializeField] PlayerActions playerActions;

    public IDamageable Damageable { get => damageableHealth; }
    public IObstruct Obstruct { get => obstruction; }
    public IAnimatable Animatable { get => playerAnimations; }
    public IObstructionChecker ObstructionChecker { get => obstructionChecker; }
    public PlayerTurnComponent TurnComponent { get => turnComponent; }
    public FaceCamera FaceCamera { get => faceCamera; }
    public string Name;

    public void ResetCooldowns()
    {
        playerActions.ResetCooldowns();
    }
}
