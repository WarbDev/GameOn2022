using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageableWithHealthComponent : DamageableComponent, IDamageable
{
    [SerializeField] Health health;

    [SerializeField] DeathComponent deathManager;
    public override bool IsDead { get => health.CurrentHealth <= 0; }

    public override DamageLog DealDamage(Damage damage)
    {
        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);

        if (IsDead)
        {
            WaitForDamageAnimation();
        }
        return new DamageLog(this, oldHealth, newHealth, damage);
    }

    void WaitForDamageAnimation()
    {
        var iAnimator = (gameEntity as IAnimatable);
        AnimatableEntity animator;
        if (iAnimator != null)
        {
            animator = iAnimator.Animatable as AnimatableEntity;
            animator.PlayedNewAnimation += CheckAnimation;
        }
 
        void CheckAnimation(ANIMATION_ID id)
        {
            if (id == ANIMATION_ID.ENTITY_HURT)
            {
                animator.SetIdleAfterAnimationEnds(false);
                animator.PlayedNewAnimation -= CheckAnimation;
                StartCoroutine(WaitForAnimationEnd(animator));
            }
        }
        
    }

    IEnumerator WaitForAnimationEnd(IAnimatable animator)
    {
        while (animator.PlayingActiveAnimation())
        {
            yield return null;
        }
        Kill();
    }

    public override void Kill()
    {
        deathManager.Die();
    }
}
