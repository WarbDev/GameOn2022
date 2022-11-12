using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Move_ : Move
{
    private ILocate locator;
    [SerializeField] GameObject animatorObject;
    Player player;


    

    private List<Location> locations;

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


        PlayGraphics();
        
    }

    private void PlayGraphics()
    {
        GameObject animation = Instantiate(animatorObject);
        //animation.transform.position = player.transform.position;

        A_ animationManager = animation.GetComponent<A_>();

        animationManager.PlayAnimation();
    }
}