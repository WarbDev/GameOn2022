using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class SpinAnimation : EntityAnimation<SpinAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    // DURATIONS: Various durations for each animation.
    [SerializeField] float duration;
    [SerializeField] Ease ease;

    // SPRITES: If animation changes sprite, use a SpriteAnimation.
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;

    private Player player;
    private FaceCamera facer;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<SpinAnimationProperties>> AnimationFinished;

    public override void Play(SpinAnimationProperties animationProperties)
    {
        facer = animationProperties.Facer;

        facer.enabled = false;

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation
        player = animationProperties.Player;

        //currentlyPlaying.Append(player.transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360).SetRelative(true).SetEase(ease));
        currentlyPlaying.Append(player.transform.DOLocalRotate(new Vector3(0, 720, 0), duration, RotateMode.FastBeyond360).SetRelative(true).SetEase(ease));
        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        // If animation has a spriteAnimation
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        // Invoke completed once the sequence is finished.
        currentlyPlaying.OnComplete(End);
        
    }

    private void End()
    {
        facer.enabled = true;

        AnimationFinished?.Invoke(this);
    }
}


public class SpinAnimationProperties : IAnimationProperties
{
    Player player;
    FaceCamera facer;

    public SpinAnimationProperties(Player player, FaceCamera facer)
    {
        this.player = player;
        this.facer = facer;
    }

    public Player Player { get => player; }
    public FaceCamera Facer { get => facer; }
}
