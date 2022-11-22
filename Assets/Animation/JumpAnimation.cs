using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class JumpAnimation : EntityAnimation<JumpAnimationProperties>
{
    [SerializeField] Ease ease;
    [SerializeField] Ease squishEase;
    [SerializeField] float jumpStartDuration;
    [SerializeField] float jumpEndDuration;
    [SerializeField] float squishStartDuration;
    [SerializeField] float squishEndDuration;
    [SerializeField] float jumpHeight;
    float totalDuration;
    [SerializeField] Transform affectedTransform;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteSet spriteSet;
    int spriteIndex = 0;
    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<JumpAnimationProperties>> AnimationFinished;

    private void Start()
    {
        enabled = false;
        totalDuration = jumpStartDuration + jumpEndDuration + squishStartDuration + squishEndDuration;
    }

    private void Update()
    {
        // spriteIndex = AnimationUtility.CurrentSpriteIndex(spriteSet.Sprites.Count, currentlyPlaying.Elapsed(), totalDuration);
        // targetSprite.sprite = spriteSet.Sprites[spriteIndex];
    }

    public override void Pause()
    {
        enabled = false;
        if (currentlyPlaying != null)
        {
            currentlyPlaying.Pause();
        }
    }

    public override void Play(JumpAnimationProperties properties)
    {
        enabled = true;
        Sequence jumpSequence = DOTween.Sequence();
        Sequence squishSequence = DOTween.Sequence();

        jumpSequence.SetEase(ease);
        squishSequence.SetEase(squishEase);
        squishSequence.Pause();

        currentlyPlaying = jumpSequence;
        jumpSequence.Append(affectedTransform.DOMove(new Vector3((properties.StartPosition.x + properties.EndPosition.x) / 2f, properties.EndPosition.y + jumpHeight), jumpStartDuration));
        jumpSequence.Append(affectedTransform.DOMove(new Vector3(properties.EndPosition.x, properties.EndPosition.y), jumpEndDuration));
        jumpSequence.Append(affectedTransform.DOScaleY(0.8f, squishStartDuration).SetEase(squishEase));
        jumpSequence.Append(affectedTransform.DOScaleY(1f, squishEndDuration).SetEase(squishEase));
        jumpSequence.OnComplete(() => invokeComplete());

        void invokeComplete()
        {
            AnimationFinished?.Invoke(this);
        }
    }

    public override void Unpause()
    {
        enabled = true;
        if (currentlyPlaying != null)
        {
            currentlyPlaying.Play();
        }
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
}