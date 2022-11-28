using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : DeathComponent
{
    protected override void OnDeath()
    {
        // Don't die lmao
        Debug.Log("Player died, but I'm lazy so respawning isn't done yet.");
    }
}
