using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAnimation : EntityAnimation<StunAnimationProperties>
{
    Sequence currentlyPlaying;

    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    public override event Action<EntityAnimation<StunAnimationProperties>> AnimationFinished;
    [SerializeField] Transform affectedTransform;
    [SerializeField] float duration;
    [SerializeField] float intensity;
    [SerializeField] int vibrato;
    [SerializeField] AudioClip clip = null;


    public override void Play(StunAnimationProperties animationProperties)
    {
        Sequence chargeSequence = DOTween.Sequence();
        float strength = intensity;
        chargeSequence.Append(affectedTransform.DOShakePosition(duration, new Vector3(strength, strength), vibrato, 90, false, false, ShakeRandomnessMode.Harmonic)).OnComplete(onComplete);

        if (clip != null)
        {
            GlobalAudioSource.Instance.Play(clip);
        }

        currentlyPlaying = chargeSequence;
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}

public class StunAnimationProperties : IAnimationProperties
{

}