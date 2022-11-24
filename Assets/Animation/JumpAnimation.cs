using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class JumpAnimation : EntityAnimation<JumpAnimationProperties>
{
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] Ease ease;
    [SerializeField] Ease squishEase;

    [SerializeField] float jumpStartDuration;
    [SerializeField] float jumpEndDuration;
    [SerializeField] float squishStartDuration;
    [SerializeField] float squishEndDuration;
    private float SpriteDuration { get => jumpStartDuration + jumpEndDuration + squishStartDuration + squishEndDuration; }

    [SerializeField] float jumpHeight;

    [SerializeField] Transform affectedTransform;
    [SerializeField] SpriteAnimation spriteAnimation;
    // [SerializeField] SpriteSet spriteSet;
    [SerializeField] SpriteRenderer targetSprite;
    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<JumpAnimationProperties>> AnimationFinished;

    public override void Play(JumpAnimationProperties properties)
    {
        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.SetEase(ease);
        jumpSequence.Append(affectedTransform.DOMove(new Vector3((properties.StartPosition.x + properties.EndPosition.x) / 2f, properties.EndPosition.y + jumpHeight), jumpStartDuration));
        jumpSequence.Append(affectedTransform.DOMove(new Vector3(properties.EndPosition.x, properties.EndPosition.y), jumpEndDuration));
        jumpSequence.Append(affectedTransform.DOScaleY(0.8f, squishStartDuration).SetEase(squishEase));
        jumpSequence.Append(affectedTransform.DOScaleY(1f, squishEndDuration).SetEase(squishEase));

        // Join together the primary animation with the sprite animation, and announce completion when done.
        currentlyPlaying = jumpSequence;
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);
        currentlyPlaying.OnComplete(onComplete);
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}

public class JumpAnimationProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }

    public JumpAnimationProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }

    public JumpAnimationProperties(Location start, Location end)
    {
        this.startPosition = LocationUtility.LocationToVector3(start);
        this.endPosition = LocationUtility.LocationToVector3(end);
    }
}