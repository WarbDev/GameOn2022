using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

// DO NOT INHERIT DIRECTLY FROM THIS CLASS WHEN MAKING AN ANIMATION
// INHERIT FROM EntityAnimation<T> INSTEAD
public abstract class EntityAnimation : MonoBehaviour
{
    public abstract Sequence CurrentlyPlaying { get; }
    public void Pause()
    {
        if (CurrentlyPlaying != null)
        {
            CurrentlyPlaying.Pause();
        }
    }

    public void Unpause()
    {
        if (CurrentlyPlaying != null)
        {
            CurrentlyPlaying.Play();
        }
    }

}

// Inherit from this class rather than EntityAnimation
public abstract class EntityAnimation<T> : EntityAnimation where T : IAnimationProperties
{
    public abstract event Action<EntityAnimation<T>> AnimationFinished;
    public T AnimationProperties { get; }
    public abstract void Play(T animationProperties);
}

public interface IAnimationProperties
{

}

public enum ANIMATION_ID
{
    ENTITY_JUMP,
    MOVE_PROJECTILE,
    MOVE_PAYLOAD,
    ENTITY_HURT,
    ENTITY_PUSHED,
    ENTITY_ANGRY,
    ENTITY_DIE,
    ENTITY_IDLE
}