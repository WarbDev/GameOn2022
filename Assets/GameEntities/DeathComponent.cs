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

        if (HasAnimator)
        {
            StartCoroutine(VanishOnceAnimationFinished());
        }
        else OnDeath();
    }

    protected virtual void OnDeath()
    {
        Destroy(gameEntity.gameObject);
    }

    protected IEnumerator VanishOnceAnimationFinished()
    {
        while (Animator.PlayingActiveAnimation)
        {
            yield return null;
        }
        OnDeath();
    }

    protected void RemoveFromEntityCollection()
    {
        Entities.RemoveEntity(gameEntity);
    }
}
