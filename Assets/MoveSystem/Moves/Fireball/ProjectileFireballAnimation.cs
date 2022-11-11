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
    [SerializeField] SpriteSet spriteSet;

    public override event Action<EntityAnimation<PFireballAnimationProperties>> AnimationFinished;

    int spriteIndex = 0;
    Sequence currentlyPlaying;


    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        spriteIndex = AnimationUtility.CurrentSpriteIndex(spriteSet.Sprites.Count - 1, currentlyPlaying.Elapsed(), duration);
        targetSprite.sprite = spriteSet.Sprites[spriteIndex];
    }

    public override void Play(PFireballAnimationProperties animationProperties)
    {
        enabled = true;
        Sequence sequence = DOTween.Sequence();

        sequence.SetEase(ease);

        currentlyPlaying = sequence;
        sequence.Append(affectedTransform.DOMove(new Vector3(animationProperties.EndPosition.x, animationProperties.EndPosition.y), duration));
        sequence.OnComplete(() => AnimationFinished?.Invoke(this));
    }

    public override void Unpause()
    {
        enabled = false;
        if (currentlyPlaying != null) currentlyPlaying.Play();
    }
    public override void Pause()
    {
        enabled = true;
        if (currentlyPlaying != null) currentlyPlaying.Pause();
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