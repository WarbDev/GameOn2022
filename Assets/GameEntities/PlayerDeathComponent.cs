using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : DeathComponent
{
    public override void OnDeathAnimationComplete()
    {
        // Don't die lmao
        Debug.Log("Player died, but I'm lazy so respawning isn't done yet.");
    }
}
