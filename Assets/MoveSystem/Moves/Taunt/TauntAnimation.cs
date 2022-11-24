using System;
using DG.Tweening;
using UnityEngine;

public class TauntAnimation : EntityAnimation<TauntAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] float duration;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] AudioClip clip;

    public override event Action<EntityAnimation<TauntAnimationProperties>> AnimationFinished;

    public override void Play(TauntAnimationProperties animationProperties)
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


public class TauntAnimationProperties : IAnimationProperties
{

}