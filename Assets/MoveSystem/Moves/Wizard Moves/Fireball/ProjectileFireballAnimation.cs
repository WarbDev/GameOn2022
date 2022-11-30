using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ProjectileFireballAnimation : EntityAnimation<PFireballAnimationProperties>
{

    [SerializeField] Ease ease;
    [SerializeField] float duration;
    [SerializeField] Transform affectedTransform;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;

    public override event Action<EntityAnimation<PFireballAnimationProperties>> AnimationFinished;
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }


    public override void Play(PFireballAnimationProperties animationProperties)
    {
        if (animationProperties.EndPosition.x > 0)
        {
            targetSprite.flipX = true;
        }

        currentlyPlaying = DOTween.Sequence();
        currentlyPlaying.SetEase(ease);
        currentlyPlaying.Append(affectedTransform.DOMove(new Vector3(animationProperties.EndPosition.x, animationProperties.EndPosition.y), duration));

        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        currentlyPlaying.OnComplete(onComplete);
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}


public class PFireballAnimationProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }

    public PFireballAnimationProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}