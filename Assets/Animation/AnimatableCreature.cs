using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableCreature : AnimatableEntity
{
    [SerializeField] EntityAnimation<SpriteAnimationProperties> idleAnimation;
    [SerializeField] EntityAnimation<SpriteAnimationProperties> deathAnimation;
    [SerializeField] Death death;

    protected override void OnAnimationFinished<T>(EntityAnimation<T> animation)
    {
        animation.AnimationFinished -= OnAnimationFinished;
        if (death.IsDead)
        {
            // Do stuff
        }
    }
}
