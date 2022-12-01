using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : DeathComponent
{
    public override void OnDeathAnimationComplete()
    {
        transform.position = new Vector3(-1000f, -1000f, -1000f);
    }
}
