using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Lightning")]
public class Move_Lightning : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    [SerializeField] int range;
    [SerializeField] int damage;
    ShapeWithRadiusAndDirection rangeShape = LocationUtility.LocationsInLine;


    private List<Location> locations;
    List<DamageLog> log;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;

        List<Location> area = rangeShape(player.Location + Directions.E, range, Directions.E);
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
        if (locations[0].X > 0)
        {
            area = rangeShape(player.Location + Directions.E, range, Directions.E);
        }
        else
        {
            area = rangeShape(player.Location + Directions.W, range, Directions.W);
        }

        List<IDamageable> damageables = LocationUtility.GetDamageablesAtPositions(area);
        log = new();
        foreach (IDamageable enemy in damageables)
        {
            log.Add(enemy.DealDamage(new Damage(damage, player)));
        }

        PlayGraphics(area, log);

    }

    private void PlayGraphics(List<Location> area, List<DamageLog> damageLogs)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Lightning animationManager = animation.GetComponent<A_Lightning>();

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animationManager.PlayAnimation(area, damageLogs);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<LightningAnimationProperties> obj)
    {

        foreach (DamageLog damaged in log)
        {
            damaged.Target.Entity.GetComponent<IAnimatable>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}