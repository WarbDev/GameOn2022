using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeAnimation : EntityAnimation<TimeAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    // DURATIONS: Various durations for each animation.
    [SerializeField] float duration;

    // AUDIO : AudioClips
    [SerializeField] AudioClip clip;

    [SerializeField] Ease xEase;
    [SerializeField] Ease yEase;
    [SerializeField] Ease zUpEase;
    [SerializeField] Ease zDownEase;
    [SerializeField] int heightGain;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<TimeAnimationProperties>> AnimationFinished;

    public override void Play(TimeAnimationProperties animationProperties)
    {
        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        TimebombTerrain bomb = animationProperties.Bomb;
        Transform bombtransform = bomb.gameObject.transform;
        Vector3 endPoint = animationProperties.EndPoint;


        currentlyPlaying.Append(bombtransform.DOMoveX(endPoint.x, duration).SetEase(xEase));
        currentlyPlaying.Insert(0, bombtransform.DOMoveY(endPoint.y, duration).SetEase(yEase));

        currentlyPlaying.Insert(0, bombtransform.DOMoveZ(endPoint.z - heightGain, duration/2).SetEase(zUpEase));

        currentlyPlaying.Insert(duration / 2, bombtransform.DOMoveZ(endPoint.z, duration / 2).SetEase(zDownEase));

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        // Invoke completed once the sequence is finished.
        currentlyPlaying.OnComplete(onComplete);
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}


public class TimeAnimationProperties : IAnimationProperties
{
    TimebombTerrain bomb;
    Vector3 endPoint;

    public TimeAnimationProperties(TimebombTerrain bomb, Vector3 endPoint)
    {
        this.bomb = bomb;
        this.endPoint = endPoint;
    }

    public TimebombTerrain Bomb { get => bomb; }
    public Vector3 EndPoint { get => endPoint; }
}