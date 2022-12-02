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

        
        List<Action> scyth= new();
        foreach (Enemy enemy in enemies)
        {
            DamageableWithHealthComponent dam = enemy.GetComponent<DamageableWithHealthComponent>();

            damageLog.Add(dam.DealDamage(new Damage(damage, player), out Action triggerDeath));
            scyth.Add(triggerDeath);
        }

        foreach (Enemy enemy in enemies)
        {
            Location direction = enemy.Location - player.Location;

            requests.Add(new PushRequest(enemy, direction, pushStrength));
        }

        pushLog = Push.CalculatePushes(requests);
        Push.DoPushes(pushLog);

        foreach (Action die in scyth)
        {
            die?.Invoke();
        }

        PlayGraphics(enemies, scyth);

    }

    private void PlayGraphics(List<Enemy> enemies, List<Action> scyth)
    {
        GameObject animation = Instantiate(animatorObject);
        //animation.transform.position = player.transform.position;

        A_Spin animationManager = animation.GetComponent<A_Spin>();



        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    if (damageLog[i].NewHealth > 0)
        //    {
        //        enemies[i].Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(pushLog[i]));
        //    }
        //}

        //foreach (PushLog lo in pushLog)
        //{
        //    lo.MoveLog.Entity.transform.position = LocationUtility.LocationToVector3(lo.MoveLog.Entity.Location);
        //}

        

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animationManager.PlayAnimation(player, player.FaceCamera, pushLog, damageLog, scyth);

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;

        
    }

    private void MoveDone(EntityAnimation<SpinAnimationProperties> obj)
    {

        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
