using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : EntityAnimation<SpriteAnimationProperties>
{
    public override void Pause()
    {
        enabled = false;
    }

    public override void Play(SpriteAnimationProperties animationProperties)
    {
        throw new System.NotImplementedException();
    }

    public override void Unpause()
    {
        enabled = true;
    }
}

public class SpriteAnimationProperties : IAnimationProperties
{

}