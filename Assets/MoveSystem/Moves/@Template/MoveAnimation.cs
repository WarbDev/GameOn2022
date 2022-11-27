using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// Example move animation.
/// </summary>

namespace HoldTheLine.Examples
{
    public class MoveAnimation : EntityAnimation<MoveAnimationProperties>
    {
        Sequence currentlyPlaying;
        public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

        // DURATIONS: Various durations for each animation.
        [SerializeField] float durationForPartA;
        [SerializeField] float durationForPartB;

        // SPRITES: If animation changes sprite, use a SpriteAnimation.
        [SerializeField] SpriteRenderer targetSprite;
        [SerializeField] SpriteAnimation spriteAnimation;

        // AUDIO : AudioClips
        [SerializeField] AudioClip clip;
        [SerializeField] AudioClip clip2;

        // AnimationFinished must be called once currentlyPlaying finishes.
        public override event Action<EntityAnimation<MoveAnimationProperties>> AnimationFinished;

        public override void Play(MoveAnimationProperties animationProperties)
        {
            // Initialize the current sequence
            currentlyPlaying = DOTween.Sequence(); // primary animation

            // Play an audio clip if needed.
            GlobalAudioSource.Instance.Play(clip);

            // If animation has a spriteAnimation
            spriteAnimation.Play(new(targetSprite));
            currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

            // Invoke completed once the sequence is finished.
            currentlyPlaying.OnComplete(onComplete);
            void onComplete() => AnimationFinished?.Invoke(this);
        }
    }


    public class MoveAnimationProperties : IAnimationProperties
    {

    }
}
