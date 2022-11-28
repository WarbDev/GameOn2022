using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceAnimation : EntityAnimation<IceAnimationProperties>
{
    Sequence currentlyPlaying;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }

    // DURATIONS: Various durations for each animation.
    [SerializeField] float risingDuration;

    // SPRITES: If animation changes sprite, use a SpriteAnimation.
    [SerializeField] SpriteRenderer targetSprite;
    [SerializeField] SpriteAnimation spriteAnimation;

    // AUDIO : AudioClips
    [SerializeField] AudioClip castClip;
    [SerializeField] AudioClip iceClip;

    List<TerrainBase> log;

    // AnimationFinished must be called once currentlyPlaying finishes.
    public override event Action<EntityAnimation<IceAnimationProperties>> AnimationFinished;

    public override void Play(IceAnimationProperties animationProperties)
    {
        log = animationProperties.TerrainLog;

        // Initialize the current sequence
        currentlyPlaying = DOTween.Sequence(); // primary animation

        GlobalAudioSource.Instance.Play(castClip);
        spriteAnimation.Play(new(targetSprite));
        currentlyPlaying.Insert(0, spriteAnimation.CurrentlyPlaying);

        currentlyPlaying.OnComplete(iceRises);
    }

    private void iceRises()
    {
        

        if(log.Count > 0)
        {
            GlobalAudioSource.Instance.Play(iceClip);
            log[0].Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(log[0].GetComponent<SpriteRenderer>()));

            Transform transform = log[0].gameObject.transform;

            transform.DOMove(new Vector3(transform.position.x, transform.position.y - 3), risingDuration).From().OnComplete(onComplete);

            log.RemoveAt(0);

            foreach (TerrainBase ice in log)
            {
                ice.Animatable.PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(ice.GetComponent<SpriteRenderer>()));

                Transform transforme = ice.gameObject.transform;

                transforme.DOMove(new Vector3(transforme.position.x, transforme.position.y - 3), risingDuration).From();
            }

        }
        else
        {
            AnimationFinished?.Invoke(this);
        }


        void onComplete() => AnimationFinished?.Invoke(this);
    }
}


public class IceAnimationProperties : IAnimationProperties
{
    List<TerrainBase> terrainLog;

    public IceAnimationProperties(List<TerrainBase> terrainLog)
    {
        this.terrainLog = terrainLog;
    }

    public List<TerrainBase> TerrainLog { get => terrainLog;}
}