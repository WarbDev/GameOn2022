using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimatableEntity : MonoBehaviour
{
    [SerializeField] GenericDictionary<ANIMATION_ID, EntityAnimation> animations;
    public GenericDictionary<ANIMATION_ID, EntityAnimation> Animations { get => animations; }
    public EntityAnimation<T> GetAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        return Animations[id] as EntityAnimation<T>;
    }

    public EntityAnimation<T> PlayAnimation<T>(ANIMATION_ID id, T animationProperties) where T : IAnimationProperties
    {
        var animation = Animations[id] as EntityAnimation<T>;
        animation.Play(animationProperties);
        return animation;
    }

}
