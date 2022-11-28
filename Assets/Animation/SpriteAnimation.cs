using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SpriteAnimation : EntityAnimation<SpriteAnimationProperties>
{
    [SerializeField] SpriteSet spriteSet;
    [SerializeField] Ease ease;
    [SerializeField] float duration;
    [Tooltip("-1 for infinite loops")]
    [SerializeField] int loops;
    SpriteRenderer spriteRenderer;
    Sequence currentlyPlaying;
    int spriteIndex = 0;

    public SpriteSet Set { get => spriteSet; }
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    public override event Action<EntityAnimation<SpriteAnimationProperties>> AnimationFinished;

    private void Update()
    {
        if (spriteRenderer)
        {
            spriteRenderer.sprite = spriteSet.Sprites[spriteIndex];
        }
        else enabled = false;
    }

    public override void Play(SpriteAnimationProperties animationProperties)
    {
        enabled = true;
        
        spriteRenderer = animationProperties.TargetRenderer;
        currentlyPlaying = DOTween.Sequence().SetEase(ease).SetLoops(loops).OnKill(animationFinished);
        currentlyPlaying.OnComplete(animationFinished);
        currentlyPlaying.Append(DOTween.To(() => spriteIndex, x => spriteIndex = x, spriteSet.Sprites.Count - 1, duration));

        void animationFinished()
        {
            AnimationFinished?.Invoke(this);
            spriteRenderer = null;
        }

    }
}

public class SpriteAnimationProperties : IAnimationProperties
{
    SpriteRenderer targetRenderer;

    public SpriteRenderer TargetRenderer { get => targetRenderer; }
    public SpriteAnimationProperties(SpriteRenderer targetRenderer)
    {
        this.targetRenderer = targetRenderer;
    }
}