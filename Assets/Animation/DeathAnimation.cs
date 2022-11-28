using UnityEngine;
using System;
using DG.Tweening;

public class DeathAnimation : EntityAnimation<HurtAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    // DURATIONS: Various durations for each animation.
    [SerializeField] float duration = 0.5f;
    [SerializeField] Ease ease;
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
        currentlyPlaying.Append(DOTween.To(() => color, x => color = x, new Color(color.r, color.g, color.b, 0), duration));

        // Invoke completed once the sequence is finished.
        currentlyPlaying.OnComplete(onComplete);
        void onComplete()
        {
            AnimationFinished?.Invoke(this);
        }

    }
}