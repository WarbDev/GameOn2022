using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoomFireballAnimation : EntityAnimation<BFireballAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] AudioClip clip;

    public override event Action<EntityAnimation<BFireballAnimationProperties>> AnimationFinished;


    public override void Play(BFireballAnimationProperties animationProperties)
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


public class BFireballAnimationProperties : IAnimationProperties
{

}