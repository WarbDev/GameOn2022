using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class EnemyMeleeAttackAnimation : EntityAnimation<HurtAnimationProperties>
{
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    [SerializeField] JumpCollideAnimation jumpCollideAnimation;
    [SerializeField] Transform affectedTransform;

    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<HurtAnimationProperties>> AnimationFinished;

    public override void Play(HurtAnimationProperties properties)
    {
        Sequence jumpSequence = DOTween.Sequence();

        JumpCollideProperties jumpColProp = new(properties.Log.Damage.DamageSource.Location, properties.Log.Target.Entity.Location);
        jumpColProp.OnCollide += onCollide;
        jumpCollideAnimation.Play(jumpColProp);
        jumpCollideAnimation.AnimationFinished += onComplete;

        void onCollide()
        {
            jumpColProp.OnCollide -= onCollide;
            var animatable = properties.Log.Target.Entity as IAnimatable;
            animatable.PlayAnimation(ANIMATION_ID.ENTITY_HURT, properties);
        }
        void onComplete(EntityAnimation<JumpCollideProperties> ani)
        {
            jumpCollideAnimation.AnimationFinished -= onComplete;
            AnimationFinished?.Invoke(this);
        }
        
    }
}
