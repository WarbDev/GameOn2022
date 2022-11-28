using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GravityAnimation : EntityAnimation<GravityAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    [SerializeField] Ease ease;
    // DURATIONS: Various durations for each animation.
    [SerializeField] float duration;

    // SPRITES: If animation changes sprite, use a SpriteAnimation.
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] SpriteAnimation spriteAnimationBoom;

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clipBoom;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<GravityAnimationProperties>> AnimationFinished;

    public override void Play(GravityAnimationProperties animationProperties)
    {
        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence();
        currentlyPlaying.SetEase(ease);
        currentlyPlaying.Append(gameObject.transform.DOMove(new Vector3(animationProperties.EndPosition.x, animationProperties.EndPosition.y), duration));

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        // If animation has a spriteAnimation
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);


        currentlyPlaying.OnComplete(Explode);
    }

    private void Explode()
    {
        spriteAnimationBoom.Play(new(targetSprite));

        GlobalAudioSource.Instance.Play(clipBoom);

        spriteAnimationBoom.AnimationFinished += onComplete;

        void onComplete(EntityAnimation<SpriteAnimationProperties> s) => AnimationFinished?.Invoke(this);
    }
}


public class GravityAnimationProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }

    public GravityAnimationProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}
