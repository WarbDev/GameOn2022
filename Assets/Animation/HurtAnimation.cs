using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HurtAnimation : EntityAnimation<HurtAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    // DURATIONS: Various durations for each animation.
    [SerializeField] float flashDuration;
    [SerializeField] float flashLoops;
    [SerializeField] Ease ease;
    [SerializeField] DefaultSprite defaultSprite;
    [SerializeField] SpriteRenderer targetSprite;

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<HurtAnimationProperties>> AnimationFinished;

    public override void Play(HurtAnimationProperties animationProperties)
    {
        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation
        currentlyPlaying.SetEase(ease);
        Color color = targetSprite.color;
        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        for (int i = 0; i < flashLoops; i++)
        {
            currentlyPlaying.Append(DOTween.To(() => Color.white, x => targetSprite.color = x, Color.red, flashDuration / 2));
            currentlyPlaying.Append(DOTween.To(() => Color.red, x => targetSprite.color = x, Color.white, flashDuration / 2));
        }

        // Invoke completed once the sequence is finished.
        currentlyPlaying.OnComplete(onComplete);
        void onComplete() 
        {
            defaultSprite.ResetToDefaultColor();
            AnimationFinished?.Invoke(this);
        }
        
    }
}

public class HurtAnimationProperties : IAnimationProperties
{
    DamageLog damageLog;
    public DamageLog Log { get => damageLog; }

    public HurtAnimationProperties(DamageLog damageLog)
    {
        this.damageLog = damageLog;
    }
}