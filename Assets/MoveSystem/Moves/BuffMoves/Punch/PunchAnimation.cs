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
    Enemy enemy;

    public override event Action<EntityAnimation<PunchAnimationProperties>> AnimationFinished;

    public override void Play(PunchAnimationProperties animationProperties)
    {
        enemy = animationProperties.Enemy;
        log = animationProperties.DamageLog;

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        //currentlyPlaying.Append(animationProperties.PushLog[0].MoveLog.Entity.transform.DOMove(LocationUtility.LocationToVector3(animationProperties.PushLog[0].MoveLog.End), duration).SetEase(ease));

        var animate = animationProperties.Enemy.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(animationProperties.PushLog[0]));

        animate.AnimationFinished += Explode;

        // Invoke completed once the sequence is finished.
        //currentlyPlaying.OnComplete(Explode);
    }

    private void Explode(EntityAnimation a)
    {
        currentlyPlaying = DOTween.Sequence();

        gameObject.transform.position = enemy.transform.position;

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
    Enemy enemy;

    public Vector3 EndPosition { get => endPosition; }
    public List<DamageLog> DamageLog { get => damageLog; }
    public List<PushLog> PushLog { get => pushLog; }
    public Enemy Enemy { get => enemy; }

    public PunchAnimationProperties(Vector3 endPosition, List<DamageLog> damageLog, List<PushLog> pushLog, Enemy enemy)
    {
        this.endPosition = endPosition;
        this.damageLog = damageLog;
        this.pushLog = pushLog;
        this.enemy = enemy;
    }
}