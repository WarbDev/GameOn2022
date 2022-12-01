using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAnimation : EntityAnimation<HurtAnimationProperties>
{
    [SerializeField] Bullet bulletObject;
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] AudioClip clip;
    [SerializeField] float timeTillProjectileFires;
    [SerializeField] SpriteAnimation spriteAnimation;
    [SerializeField] Vector3 bulletDistanceModification;

    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    private Sequence currentlyPlaying;
    private HurtAnimationProperties props;

    public override event Action<EntityAnimation<HurtAnimationProperties>> AnimationFinished;

    public override void Play(HurtAnimationProperties animationProperties)
    {
        props = animationProperties;

        currentlyPlaying = DOTween.Sequence();

        spriteAnimation.Play(new(myRenderer));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);


        StartCoroutine(Bullet());
        //currentlyPlaying.OnComplete();
    }

    private IEnumerator Bullet()
    {
        yield return new WaitForSeconds(timeTillProjectileFires);

        Bullet bullet = Instantiate(bulletObject);
        if (transform.position.x > 0)
        {
            bulletDistanceModification = new Vector3(-bulletDistanceModification.x, bulletDistanceModification.y, bulletDistanceModification.z);
            bullet.transform.position = transform.position + bulletDistanceModification;
        }
        else
        {
            bullet.transform.position = transform.position + bulletDistanceModification;
        }
        

        Vector3 distance = props.Log.Target.Entity.transform.position - transform.position;
        float time = MathF.Sqrt(Mathf.Pow(distance.x, 2) + Mathf.Pow(distance.y, 2));
        Vector3 modify = new Vector3(0, 1);


        bullet.transform.DOMove(props.Log.Target.Entity.transform.position + modify, time/6).OnComplete(shot);
        bullet.PlayAnimation();

        void shot()
        {
            Destroy(bullet.gameObject);
            IAnimatable animatable = (IAnimatable)props.Log.Target.Entity;
            animatable.PlayAnimation(ANIMATION_ID.ENTITY_HURT, props);

            AnimationFinished?.Invoke(this);
        }
    }
}