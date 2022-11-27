using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchAnimation : EntityAnimation<PunchAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    [SerializeField] float duration;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteSet spriteSet;
    [SerializeField] AudioClip clip;

    float startTime;
    int spriteIndex = 0;

    public override event Action<EntityAnimation<PunchAnimationProperties>> AnimationFinished;

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

    public override void Play(PunchAnimationProperties animationProperties)
    {
        transform.position = animationProperties.EndPosition;
        enabled = true;
        startTime = Time.time;
        GlobalAudioSource.Instance.Play(clip);
    }
}


public class PunchAnimationProperties : IAnimationProperties
{
    Vector3 endPosition;
    public Vector3 EndPosition { get => endPosition; }

    public PunchAnimationProperties(Vector3 endPosition)
    {
        this.endPosition = endPosition;
    }
}