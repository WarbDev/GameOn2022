using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Health))]
public class DamageableWithHealthComponent : DamageableComponent, IDamageable
{
    [SerializeField] Health health;
    [SerializeField] bool mostRecentDamageWasAnimated = true;
    [SerializeField] AnimatableEntity animatableEntity;
    [SerializeField] DeathComponent deathManager;
    public override bool IsDead { get => health.CurrentHealth <= 0; }

    public override DamageLog DealDamage(Damage damage)
    {
        mostRecentDamageWasAnimated = false;
        animatableEntity.PlayedNewAnimation += checkIfHurtPlayed;

        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);

        if (IsDead)
        {
            GameFlow.DeathAnimationTick += DoDeathAnimation;
        }
        return new DamageLog(this, oldHealth, newHealth, damage);

        void checkIfHurtPlayed(ANIMATION_ID id)
        {
            if (id == ANIMATION_ID.ENTITY_HURT)
            {
                mostRecentDamageWasAnimated = true;
                animatableEntity.PlayedNewAnimation -= checkIfHurtPlayed;
            }
        }
    }

    void DoDeathAnimation()
    {
        GameFlow.DeathAnimationTick -= DoDeathAnimation;

        StartCoroutine(PlayDeathAnimation());

        
        IEnumerator PlayDeathAnimation()
        {
            yield return new WaitUntil(() => mostRecentDamageWasAnimated);
            yield return new WaitUntil(() => !animatableEntity.PlayingActiveAnimation());
            animatableEntity.SetIdleAfterAnimationEnds(false);
            animatableEntity.PlayAnimation(ANIMATION_ID.ENTITY_DIE, new HurtAnimationProperties(new DamageLog(null, 0, 0, null)));
            StartCoroutine(WaitForAnimationEnd(animatableEntity));
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
