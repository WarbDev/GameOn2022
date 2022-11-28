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
    [SerializeField] Ease ease;
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteSet spriteSet;
    [SerializeField] AudioClip clip;

    [SerializeField] SpriteAnimation spriteAnimation;

    List<DamageLog> log;

    public override event Action<EntityAnimation<PunchAnimationProperties>> AnimationFinished;

    public override void Play(PunchAnimationProperties animationProperties)
    {
        log = animationProperties.DamageLog;

        transform.position = LocationUtility.LocationToVector3(animationProperties.PushLog[0].MoveLog.End);

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        currentlyPlaying.Append(animationProperties.PushLog[0].MoveLog.Entity.transform.DOMove(LocationUtility.LocationToVector3(animationProperties.PushLog[0].MoveLog.End), duration).SetEase(ease));

        

        // Invoke completed once the sequence is finished.
        currentlyPlaying.OnComplete(Explode);
    }

    private void Explode()
    {
        currentlyPlaying = DOTween.Sequence();

        // If animation has a spriteAnimation
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        foreach (DamageLog damaged in log)
        {
            damaged.Target.Entity.GetComponent<IAnimatable>().PlayAnimation(ANIMATION_ID.ENTITY_HURT, new HurtAnimationProperties(damaged));
        }

        currentlyPlaying.OnComplete(onComplete);
        void onComplete() => AnimationFinished?.Invoke(this);
    }
}


public class PunchAnimationProperties : IAnimationProperties
{
    Vector3 endPosition;
    List<DamageLog> damageLog;
    List<PushLog> pushLog;

    public Vector3 EndPosition { get => endPosition; }
    public List<DamageLog> DamageLog { get => damageLog; }
    public List<PushLog> PushLog { get => pushLog; }

    public PunchAnimationProperties(Vector3 endPosition, List<DamageLog> damageLog, List<PushLog> pushLog)
    {
        this.endPosition = endPosition;
        this.damageLog = damageLog;
        this.pushLog = pushLog;
    }
}