using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Taunt")]

public class Move_Taunt : Move
{

    private ILocate locator;
    [SerializeField] int range;
    [SerializeField] int force;
    [SerializeField] GameObject AnimatorObject;
    public int Range { get => range; }
    ShapeWithRadiusAndDirection rangeShape = LocationUtility.LocationsInLine;
    Player player;

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
        List<PushLog> log = new();
        List<PushRequest> requests = new();
        foreach (Enemy enemy in enemies)
        {
            Location direction = Directions.E;
            if (enemy.Location.X > 0)
            {
                direction = Directions.W;
            }
            

            requests.Add(new PushRequest(enemy, direction, force));
        }

        log = Push.CalculatePushes(requests);
        Push.DoPushes(log);

        PlayGraphics(log, enemies);

    }


    private void PlayGraphics(List<PushLog> log, List<Enemy> enemies)
    {
        GameObject animator = Instantiate(AnimatorObject);

        Vector3 difference = new Vector3(0, 1.5f);

        animator.transform.position = player.transform.position + difference;

        A_Taunt animation = animator.GetComponent<A_Taunt>();

        foreach (PushLog lo in log)
        {
            IAnimatable an = (IAnimatable)lo.MoveLog.Entity;

            an.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(lo));
        }

        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    enemies[i].Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(log[i]));
        //}

        player.Animatable.PlayAnimation(ANIMATION_ID.PLAYER_ATTACK, new SpriteAnimationProperties(player.FaceCamera.Sprite));
        animation.PlayAnimation(log);

        animation.animate.AnimationFinished -= MoveDone;
        animation.animate.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<TauntAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
