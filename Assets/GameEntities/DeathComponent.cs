using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathComponent : EntityComponent
{
    [Header("Animation Info")]
    [SerializeField] bool HasAnimator;
    [SerializeField] AnimatableEntity Animator;

    public void Die()
    {
        RemoveFromEntityCollection();
    }

    public virtual void OnDeathAnimationComplete()
    {
        Destroy(gameEntity.gameObject);
    }

    protected void RemoveFromEntityCollection()
    {
        Entities.RemoveEntity(gameEntity);
    }
}
