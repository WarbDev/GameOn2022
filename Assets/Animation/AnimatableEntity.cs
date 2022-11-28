using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class AnimatableEntity : MonoBehaviour, IAnimatable
{

    [SerializeField] GenericDictionary<ANIMATION_ID, EntityAnimation> animations;
    public GenericDictionary<ANIMATION_ID, EntityAnimation> Animations { get => animations; }
    public IAnimatable Animatable { get => this; }

    [Tooltip("Is true if playing an animation that is not an idle animation.")]
    [SerializeField] bool playingActiveAnimation = false;
    public bool PlayingActiveAnimation { get => playingActiveAnimation; }
    [SerializeField] Sequence activeIdleSequence = null;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Tooltip("If enabled, the entity will automatically play its idle animation on Start.")]
    [SerializeField] public bool IdleOnStart = false;

    [Tooltip("If enabled, the entity will automatically play its idle animation after any non-idle animation ends.")]
    [SerializeField] public bool IdleAfterAnimationEnds = false;


    void Start()
    {
        if (IdleOnStart)
        {
            PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(spriteRenderer));
        }
    }

    public virtual EntityAnimation<T> PlayAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        if (HasAnimation(id))
        {
            EntityAnimation<T> animation;

            // If this is an idle animation, assign the idleAnimation to the new animation.
            if (id == ANIMATION_ID.ENTITY_IDLE)
            {
                animation = PlayValidAnimation(id, animationProperties);
                animation.AnimationFinished += OnIdleAnimationFinished;
                activeIdleSequence = animation.CurrentlyPlaying;
            }

            // If not an idle animation, cancel any playing idleAnimation and unassign it.
            else
            {
                playingActiveAnimation = true;
                animation = PlayValidAnimation(id, animationProperties);
                animation.AnimationFinished += OnAnimationFinished;

                if (activeIdleSequence != null)
                {
                    activeIdleSequence.Complete();
                    activeIdleSequence = null;
                }
            }
            
            return animation;
        }
            

        else
            return PlayBadAnimation<T>();
    }

    protected EntityAnimation<T> PlayValidAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        var animation = Animations[id] as EntityAnimation<T>;
        animation.Play(animationProperties);
        animation.AnimationFinished += OnAnimationFinished;
        return animation;
    }

    protected EntityAnimation<T> PlayBadAnimation<T>() where T : IAnimationProperties
    {
        EntityAnimation<BadAnimationProperties> animation = BadAnimation.Instance;
        animation.Play(new());
        animation.AnimationFinished += OnAnimationFinished;
        return animation as EntityAnimation<T>;
    }

    protected virtual void OnAnimationFinished<T>(EntityAnimation<T> animation) where T : IAnimationProperties
    {
        playingActiveAnimation = false;
        animation.AnimationFinished -= OnAnimationFinished;

        if (IdleAfterAnimationEnds)
        {
            PlayAnimation(ANIMATION_ID.ENTITY_IDLE, new SpriteAnimationProperties(spriteRenderer));
        }
        
    }

    protected virtual void OnIdleAnimationFinished<T>(EntityAnimation<T> animation) where T: IAnimationProperties
    {
        animation.AnimationFinished -= OnIdleAnimationFinished;
    }

    public bool HasAnimation(ANIMATION_ID id)
    {
        return Animations.ContainsKey(id);
    }
}

public interface IAnimatable
{
    /// <summary>
    /// Implementor of the default methods.
    /// </summary>
    public IAnimatable Animatable { get; }

    // Must be implemented by the Animatable reference.
    public bool HasAnimation(ANIMATION_ID id)
    {
        return Animatable.HasAnimation(id);
    }

    // Must be implemented by the Animatable reference.
    public EntityAnimation<T> PlayAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        return Animatable.PlayAnimation(id, animationProperties);
    }
}