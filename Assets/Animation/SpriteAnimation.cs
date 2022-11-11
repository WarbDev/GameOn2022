using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteAnimation : EntityAnimation<SpriteAnimationProperties>
{
    [SerializeField] SpriteSet spriteSet;
    Coroutine spriteUpdateCoroutine;

    public override event Action<EntityAnimation<SpriteAnimationProperties>> AnimationFinished;

    void Start()
    {
        StartCoroutine(UpdateSprite());    
    }

    public override void Pause()
    {
        enabled = false;
    }

    public override void Play(SpriteAnimationProperties animationProperties)
    {
        spriteUpdateCoroutine = StartCoroutine(UpdateSprite());
    }

    IEnumerator UpdateSprite()
    {
        while (true)
        {

        }
    }

    public override void Unpause()
    {
        enabled = true;
    }
}

public class SpriteAnimationProperties : IAnimationProperties
{
    SpriteRenderer targetRenderer;
    Func<float> timeElapsedCalc;
    float duration;

    public SpriteRenderer TargetRenderer { get => targetRenderer; }
    public Func<float> TimeElapsedCalc { get => timeElapsedCalc; }
    public float Duration { get => duration; }
    public SpriteAnimationProperties(SpriteRenderer targetRenderer, Func<float> timeElapsedCalc, float duration)
    {
        this.targetRenderer = targetRenderer;
        this.timeElapsedCalc = timeElapsedCalc;
        this.duration = duration;
    }
}