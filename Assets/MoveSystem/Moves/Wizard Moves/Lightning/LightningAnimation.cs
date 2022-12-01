using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class LightningAnimation : EntityAnimation<LightningAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }


    // SPRITES: If animation changes sprite, use a SpriteAnimation.
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] int lengthToWidthRatio; //if the sprite is 6 times longer than it is wide, set this to 6

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<LightningAnimationProperties>> AnimationFinished;


    public override void Play(LightningAnimationProperties animationProperties)
    {
        float scale = (float) animationProperties.area.Count / (float) lengthToWidthRatio;
        gameObject.transform.localScale += new Vector3(scale, 1, 1);

        int direction = LocationUtility.DirectionTowardsCenter(animationProperties.area[0]);
        if (direction > -1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        // If animation has a spriteAnimation
        spriteAnimation.Play(new(targetSprite));
        spriteAnimation.AnimationFinished += onComplete;
        //currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        // Invoke completed once the sequence is finished.
        //currentlyPlaying.OnComplete(onComplete);
        void onComplete(EntityAnimation e) => AnimationFinished?.Invoke(this);
    }
}


public class LightningAnimationProperties : IAnimationProperties
{
    public List<Location> area;

    public LightningAnimationProperties(List<Location> area)
    {
        this.area = area;
    }
}