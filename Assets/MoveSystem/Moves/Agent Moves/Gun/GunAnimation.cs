using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class GunAnimation : EntityAnimation<GunAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }


    [SerializeField] Ease ease;
    [SerializeField] float duration;

    // SPRITES: If animation changes sprite, use a SpriteAnimation.
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] SpriteAnimation smallExplodeAnimation;

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;
    [SerializeField] AudioClip clipShot;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<GunAnimationProperties>> AnimationFinished;

    public override void Play(GunAnimationProperties animationProperties)
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



        currentlyPlaying.OnComplete(smallExplosion);
        
    }

    private void smallExplosion()
    {
        smallExplodeAnimation.Play(new(targetSprite));

        GlobalAudioSource.Instance.Play(clipShot);

        smallExplodeAnimation.AnimationFinished += onComplete;

        void onComplete(EntityAnimation<SpriteAnimationProperties> s) => AnimationFinished?.Invoke(this);
    }



}


public class GunAnimationProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }

    public GunAnimationProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}

