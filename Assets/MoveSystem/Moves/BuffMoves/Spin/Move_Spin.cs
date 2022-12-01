using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Moves/Spin")]

public class Move_Spin : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;

    ShapeWithRadius rangeShape = LocationUtility.LocationsInAllCardinalAndOrdinalDirections;
    [SerializeField] int pushStrength;
    [SerializeField] int damage;

    List<DamageLog> damageLog;
    List<PushLog> pushLog;

    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;

        List<Location> area = rangeShape(player.Location, 1);
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

        locations = rangeShape(player.Location, 1);

        List<Enemy> enemies = LocationUtility.GetEnemiesInPositions(locations);
        damageLog = new();
        pushLog = new();
        List<PushRequest> requests = new();

        foreach (Enemy enemy in enemies)
        {
            Location direction = enemy.Location - player.Location;

            requests.Add(new PushRequest(enemy, direction, pushStrength));
        }

        pushLog = Push.CalculatePushes(requests);
        Push.DoPushes(pushLog);

        foreach (Enemy enemy in enemies)
        {
            damageLog.Add(enemy.Damageable.DealDamage(new Damage(damage, player)));
        }

        

        PlayGraphics(enemies);

    }

    private void PlayGraphics(List<Enemy> enemies)
    {
        GameObject animation = Instantiate(animatorObject);
        //animation.transform.position = player.transform.position;

        A_Spin animationManager = animation.GetComponent<A_Spin>();

        //foreach (PushLog log in pushLog)
        //{
        //    IAnimatable an = (IAnimatable)log.MoveLog.Entity;

        //    an.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(log));
        //}

        for (int i = 0; i < enemies.Count; i++)
        {
            if (damageLog[i].NewHealth > 0)
            {
                enemies[i].Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(pushLog[i]));
            }
        }

        //foreach (PushLog lo in pushLog)
        //{
        //    lo.MoveLog.Entity.transform.position = LocationUtility.LocationToVector3(lo.MoveLog.Entity.Location);
        //}
        foreach (DamageLog damaged in damageLog)
        {
            damaged.Target.Entity.GetComponent<IAnimatable>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }
        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animationManager.PlayAnimation(player, player.FaceCamera);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<SpinAnimationProperties> obj)
    {

        

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
