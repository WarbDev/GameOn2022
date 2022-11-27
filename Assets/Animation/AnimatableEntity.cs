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

    public virtual EntityAnimation<T> PlayAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        if (HasAnimation(id))
            return PlayValidAnimation(id, animationProperties);

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
        animation.AnimationFinished -= OnAnimationFinished;
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