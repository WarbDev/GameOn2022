using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// Animation that jumps partially onto a tile and then collides and falls back to its original spot.
/// </summary>
public class JumpCollideAnimation : EntityAnimation<JumpCollideProperties>
{
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] Ease ease;
    [SerializeField] Ease squishEase;

    [SerializeField] float jumpStartDuration;
    [SerializeField] float jumpEndDuration;
    [SerializeField] float distanceRatio = 5;

    [SerializeField] float jumpHeight;
    [SerializeField] Transform affectedTransform;
    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<JumpCollideProperties>> AnimationFinished;

    public override void Play(JumpCollideProperties properties)
    {
        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.Append(affectedTransform.DOMove(new Vector3((properties.StartPosition.x + properties.EndPosition.x * distanceRatio) / (1 + distanceRatio), properties.EndPosition.y + jumpHeight), jumpStartDuration).SetEase(ease).OnComplete(properties.InvokeCollide));
        jumpSequence.Append(affectedTransform.DOPunchPosition(new Vector3(0f, 0.2f, 0f), 0.4f, 2, 0.5f).SetEase(squishEase));
        jumpSequence.Append(affectedTransform.DOMove(new Vector3(properties.StartPosition.x, properties.StartPosition.y), jumpEndDuration).SetEase(ease)).OnComplete(onComplete);
        

        // Join together the primary animation with the sprite animation, and announce completion when done.
        currentlyPlaying = jumpSequence;
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}

public class JumpCollideProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;
    public event Action OnCollide;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }

    public JumpCollideProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }

    public JumpCollideProperties(Location start, Location end)
    {
        this.startPosition = LocationUtility.LocationToVector3(start);
        this.endPosition = LocationUtility.LocationToVector3(end);
    }

    public void InvokeCollide()
    {
        OnCollide?.Invoke();
    }
}