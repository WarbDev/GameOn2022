using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// Animation that jumps partially onto a tile and then collides and falls back to its original spot.
/// </summary>
public class ChargeAnimation : EntityAnimation<ChargeAnimationProperties>
{
    [InspectorButton("TestPlay")]
    [SerializeField] bool play;
    public void TestPlay()
    {
        Play(new(testCharge));
    }

    [SerializeField] int testCharge = 0;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] Ease ease;
    [SerializeField] Transform affectedTransform;
    [SerializeField] float duration;
    [SerializeField] float intensityScale = 0;
    [SerializeField] float intensity = 0;
    [SerializeField] int vibrato = 0;
    [SerializeField] List<AudioClip> clips;

    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<ChargeAnimationProperties>> AnimationFinished;

    

    public override void Play(ChargeAnimationProperties properties)
    {
        Sequence chargeSequence = DOTween.Sequence();
        float strength = intensity + (properties.TimesCharged * intensityScale);
        chargeSequence.Append(affectedTransform.DOShakePosition(duration + 1 * intensityScale, new Vector3(strength, strength), vibrato, 90, false, true, ShakeRandomnessMode.Harmonic));
        GlobalAudioSource.Instance.Play(clips[properties.TimesCharged]);


        // Join together the primary animation with the sprite animation, and announce completion when done.
        currentlyPlaying = chargeSequence;

        StartCoroutine(endMe());
    }

    private IEnumerator endMe()
    {
        yield return new WaitForSeconds(duration/2);
        AnimationFinished?.Invoke(this);
    }
}

public class ChargeAnimationProperties : IAnimationProperties
{
    public int TimesCharged;
    public ChargeAnimationProperties(int charged)
    {
        TimesCharged = charged;
    }
}