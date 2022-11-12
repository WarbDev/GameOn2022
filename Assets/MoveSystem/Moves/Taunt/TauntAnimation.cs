using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TauntAnimation : EntityAnimation<TauntAnimationProperties>
{

    [SerializeField] float duration;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteSet spriteSet;

    float startTime;
    int spriteIndex = 0;

    public override event Action<EntityAnimation<TauntAnimationProperties>> AnimationFinished;

    // Start is called before the first frame update
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

    public override void Play(TauntAnimationProperties animationProperties)
    {
        enabled = true;
        startTime = Time.time;
    }

    public override void Unpause()
    {
        enabled = true;
    }
    public override void Pause()
    {
        enabled = false;
    }
}


public class TauntAnimationProperties : IAnimationProperties
{

}