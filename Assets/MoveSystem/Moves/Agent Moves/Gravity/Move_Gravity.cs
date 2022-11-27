using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Moves/Gravity")]
public class Move_Gravity : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;




    private List<Location> locations;

    public override event Action<bool> MoveCompleted;

    public override void DoMove(Player player)
    {
        this.player = player;
        //locator = new Locator
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

        PlayGraphics();

    }

    private void PlayGraphics()
    {
        GameObject animation = Instantiate(animatorObject);
        //animation.transform.position = player.transform.position;

        A_Gravity animationManager = animation.GetComponent<A_Gravity>();

        animationManager.PlayAnimation();

        animationManager.moveAnimation.AnimationFinished -= MoveDone;
        animationManager.moveAnimation.AnimationFinished += MoveDone;
    }

    private void MoveDone(EntityAnimation<GravityAnimationProperties> obj)
    {
        obj.AnimationFinished -= MoveDone;
        MoveCompleted?.Invoke(true);
    }
}
