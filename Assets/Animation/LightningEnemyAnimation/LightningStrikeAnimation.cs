using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class LightningStrikeAnimation : EntityAnimation<LightningStrikeProperties>
{
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] AudioClip clip;
    [SerializeField] SpriteAnimation spriteAnimation;

    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    private Sequence currentlyPlaying;
    private LightningStrikeProperties props;

    public override event Action<EntityAnimation<LightningStrikeProperties>> AnimationFinished;

    public override void Play(LightningStrikeProperties animationProperties)
    {
        props = animationProperties;

        currentlyPlaying = DOTween.Sequence();

        GlobalAudioSource.Instance.Play(clip);

        spriteAnimation.Play(new(myRenderer));
        myRenderer.sortingLayerName = "Barks";
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);


        StartCoroutine(End());
    }


    private IEnumerator End()
    {
        yield return new WaitForSeconds(.5f);
        foreach (var damageLog in props.damageLogs)
        {
            IAnimatable animatable = damageLog.Target.Entity as IAnimatable;
            animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENTITY_HURT, new(damageLog));
        }
        yield return new WaitForSeconds(.5f);
        myRenderer.sortingLayerName = "Entities";
        AnimationFinished?.Invoke(this);
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

/*
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

    private LightningStrikeProperties props;

    public override void Play(LightningStrikeProperties properties)
    {
        props = properties;
        currentlyPlaying = DOTween.Sequence();

        ChargeAnimationProperties chargeProp = new(2);
        //chargeAnimation.Play(chargeProp);
        //chargeAnimation.AnimationFinished += onComplete;


        // Join together the primary animation with the sprite animation, and announce completion when done.
        //void onComplete(EntityAnimation<ChargeAnimationProperties> a)
        //{
            
           // a.AnimationFinished -= onComplete;

            gameObject.SetActive(true);
            enemySpriteAnimation.Play(new(enemySpriteRenderer));

            
            StartCoroutine(LightningBolt());

            enemySpriteAnimation.AnimationFinished += finished;

            void finished(EntityAnimation e)
            {
                AnimationFinished?.Invoke(this);
                gameObject.SetActive(false);
            }
        //} 
    }

    private IEnumerator LightningBolt()
    {
        yield return new WaitForSeconds(.5f);

        foreach (var damageLog in props.damageLogs)
        {
            IAnimatable animatable = damageLog.Target.Entity as IAnimatable;
            animatable.PlayAnimation<HurtAnimationProperties>(ANIMATION_ID.ENTITY_HURT, new(damageLog));
        }

        lightningSpriteAnimation.Play(new(gameObject.GetComponent<SpriteRenderer>()));
        lightningSpriteAnimation.AnimationFinished += finished;


        void finished(EntityAnimation e)
        {
            
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
*/