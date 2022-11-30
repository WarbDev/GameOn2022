using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerAnimation : EntityAnimation<SpriteAnimationProperties>
{
    Sequence currentlyPlaying;
    [SerializeField] SpriteAnimation animate;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    public override event Action<EntityAnimation<SpriteAnimationProperties>> AnimationFinished;


    public override void Play(SpriteAnimationProperties animationProperties)
    {
        animate.Play(animationProperties);
    }
}