using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindAnimation : EntityAnimation<WindAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] AudioClip clip;

    public override event Action<EntityAnimation<WindAnimationProperties>> AnimationFinished;

    public override void Play(WindAnimationProperties animationProperties)
    {
        GlobalAudioSource.Instance.Play(clip);

        // Join together the primary animation with the sprite animation, and announce completion when done.
        currentlyPlaying = DOTween.Sequence(); // primary animation
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        currentlyPlaying.OnComplete(onComplete);
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}

public class WindAnimationProperties : IAnimationProperties
{

}