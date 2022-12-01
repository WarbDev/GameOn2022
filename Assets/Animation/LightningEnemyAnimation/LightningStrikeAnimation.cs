using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class LightningStrikeAnimation : EntityAnimation<LightningStrikeProperties>
{
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    [SerializeField] Transform affectedTransform;
    [SerializeField] SpriteRenderer enemySpriteRenderer;
    [SerializeField] SpriteAnimation enemySpriteAnimation;
    [SerializeField] ChargeAnimation chargeAnimation;
    [SerializeField] AudioClip clip;
    
    [SerializeField] SpriteAnimation lightningSpriteAnimation;
    Sequence currentlyPlaying;

    public override event Action<EntityAnimation<LightningStrikeProperties>> AnimationFinished;

    public override void Play(LightningStrikeProperties properties)
    {
        currentlyPlaying = DOTween.Sequence();

        ChargeAnimationProperties chargeProp = new(2);
        chargeAnimation.Play(chargeProp);
        chargeAnimation.AnimationFinished += onComplete;


        // Join together the primary animation with the sprite animation, and announce completion when done.
        void onComplete(EntityAnimation<ChargeAnimationProperties> a)
        {
            foreach (var damageLog in properties.damageLogs)
            {
                IAnimatable animatable = damageLog.Target.Entity as IAnimatable;
                animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENTITY_HURT, new(damageLog));
            }
            a.AnimationFinished -= onComplete;

            enemySpriteAnimation.Play(new(enemySpriteRenderer));

            StartCoroutine(LightningBolt());

            enemySpriteAnimation.AnimationFinished += finished;

            void finished(EntityAnimation e)
            {
                AnimationFinished?.Invoke(this);
            }
        } 
    }

    private IEnumerator LightningBolt()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(true);
        lightningSpriteAnimation.Play(new(gameObject.GetComponent<SpriteRenderer>()));
        lightningSpriteAnimation.AnimationFinished += finished;


        void finished(EntityAnimation e)
        {
            gameObject.SetActive(false);
        }
    }
}

public class LightningStrikeProperties : IAnimationProperties
{
    public List<DamageLog> damageLogs;

    public LightningStrikeProperties(List<DamageLog> damageLogs)
    {
        this.damageLogs = damageLogs;
    }
}
