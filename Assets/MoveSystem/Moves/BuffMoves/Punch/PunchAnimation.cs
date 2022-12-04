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
    List<Action> scyth;
    List<PushLog> push;

    public override event Action<EntityAnimation<PunchAnimationProperties>> AnimationFinished;

    EntityAnimation<PushAnimationProperties> animate;

    public override void Play(PunchAnimationProperties animationProperties)
    {
        scyth = animationProperties.scyth;
        log = animationProperties.DamageLog;
        push = animationProperties.PushLog;

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        // Play an audio clip if needed.
        GlobalAudioSource.Instance.Play(clip);

        //currentlyPlaying.Append(animationProperties.PushLog[0].MoveLog.Entity.transform.DOMove(LocationUtility.LocationToVector3(animationProperties.PushLog[0].MoveLog.End), duration).SetEase(ease));

        animate = animationProperties.Enemy.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_PUSHED, new PushAnimationProperties(animationProperties.PushLog[0], 2));

        animate.AnimationFinished -= Explode;
        animate.AnimationFinished += Explode;

        // Invoke completed once the sequence is finished.
        //currentlyPlaying.OnComplete(Explode);
    }

    private void Explode(EntityAnimation a)
    {
        animate.AnimationFinished -= Explode;

        currentlyPlaying = DOTween.Sequence();

        gameObject.transform.position = LocationUtility.LocationToVector3(push[0].MoveLog.End);

        // If animation has a spriteAnimation
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Append(spriteAnimation.CurrentlyPlaying);

        //foreach (Action die in scyth)
        //{
        //    die?.Invoke();
        //}

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
    IAnimatable enemy;
    public List<Action> scyth;

    public Vector3 EndPosition { get => endPosition; }
    public List<DamageLog> DamageLog { get => damageLog; }
    public List<PushLog> PushLog { get => pushLog; }
    public IAnimatable Enemy { get => enemy; }

    public PunchAnimationProperties(Vector3 endPosition, List<DamageLog> damageLog, List<PushLog> pushLog, IAnimatable enemy, List<Action> scyth)
    {
        this.scyth = scyth;
        this.endPosition = endPosition;
        this.damageLog = damageLog;
        this.pushLog = pushLog;
        this.enemy = enemy;
    }
}