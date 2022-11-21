using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimation : EntityAnimation<WindAnimationProperties>
{

    [SerializeField] float duration;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteSet spriteSet;
    [SerializeField] AudioClip clip;

    public override event Action<EntityAnimation<WindAnimationProperties>> AnimationFinished;
    
    float startTime;
    int spriteIndex = 0;

    public override void Pause()
    {
        enabled = false;
    }

    public override void Play(WindAnimationProperties animationProperties)
    {
        enabled = true;
        startTime = Time.time;
        GlobalAudioSource.Instance.Play(clip);
    }

    public override void Unpause()
    {
        enabled = true;
    }

    void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        spriteIndex = AnimationUtility.CurrentSpriteIndex(spriteSet.Sprites.Count, Time.time - startTime, duration);
        targetSprite.sprite = spriteSet.Sprites[spriteIndex];
        if (Time.time - startTime >= duration)
        {
            AnimationFinished?.Invoke(this);
        }
    }
}

public class WindAnimationProperties : IAnimationProperties
{

}