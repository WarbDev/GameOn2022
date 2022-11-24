using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimatableEntity : MonoBehaviour, IAnimatable
{

    [SerializeField] GenericDictionary<ANIMATION_ID, EntityAnimation> animations;
    public GenericDictionary<ANIMATION_ID, EntityAnimation> Animations { get => animations; }
    public IAnimatable Animatable { get => this; }

    public EntityAnimation<T> PlayAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        if (HasAnimation(id))
        {
            var animation = Animations[id] as EntityAnimation<T>;
            animation.Play(animationProperties);
            return animation;
        }
        else
        {
            EntityAnimation<BadAnimationProperties> animation = BadAnimation.Instance;
            animation.Play(new());
            return animation as EntityAnimation<T>;
        }
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