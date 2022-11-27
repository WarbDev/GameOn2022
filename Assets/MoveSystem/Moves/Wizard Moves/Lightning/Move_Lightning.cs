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

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(area);
        List<DamageLog> log = new();
        foreach (Enemy enemy in enemies)
        {
            log.Add(enemy.Damageable.DealDamage(new Damage(damage, player)));
        }

        PlayGraphics(area, log);

    }

    private void PlayGraphics(List<Location> area, List<DamageLog> damageLogs)
    {
        GameObject animation = Instantiate(animatorObject);
        animation.transform.position = player.transform.position;

        A_Lightning animationManager = animation.GetComponent<A_Lightning>();

        animationManager.PlayAnimation(area, damageLogs);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<LightningAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}