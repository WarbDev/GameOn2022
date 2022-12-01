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

    public event Action<int> DamageAnimationPlayed;
    public override bool IsDead { get => health.CurrentHealth <= 0; }

    public override DamageLog DealDamage(Damage damage)
    {
        mostRecentDamageWasAnimated = false;
        

        float oldHealth = health.CurrentHealth;
        float newHealth = health.ReduceHealth(damage.Base);
        animatableEntity.PlayedNewAnimation += checkIfHurtPlayed;
        if (IsDead)
        {
            
            deathManager.Die();
            GameFlow.DeathAnimationTick += DoDeathAnimation;
        }
        return new DamageLog(this, oldHealth, newHealth, damage);

        void checkIfHurtPlayed(ANIMATION_ID id)
        {
            if (id == ANIMATION_ID.ENTITY_HURT)
            {
                DamageAnimationPlayed?.Invoke((int)newHealth);
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
            
            var animation = animatableEntity.PlayAnimation(ANIMATION_ID.ENTITY_DIE, new HurtAnimationProperties(new DamageLog(null, 0, 0, null)));
            StartCoroutine(WaitForAnimationEnd(animatableEntity, animation));
        }
        
    }

    IEnumerator WaitForAnimationEnd(IAnimatable animator, EntityAnimation<HurtAnimationProperties> animation)
    {
        bool isDone = false;
        animation.AnimationFinished += onNewAnimationPlayed;
        while (!isDone)
        {
            yield return null;
        }
        Kill();

        void onNewAnimationPlayed(EntityAnimation<HurtAnimationProperties> ani)
        {
            isDone = true;
            ani.AnimationFinished -= onNewAnimationPlayed;
        }
    }

    public override void Kill()
    {
        deathManager.OnDeathAnimationComplete();
    }

/// <summary>
/// Forces a kill to immediately play the death animation.
/// </summary>
    public void Kill(bool playDeathAnimation)
    {
        StartCoroutine(PlayDeathAnimation());


        IEnumerator PlayDeathAnimation()
        {
            yield return new WaitUntil(() => !animatableEntity.PlayingActiveAnimation());
            animatableEntity.SetIdleAfterAnimationEnds(false);

            var animation = animatableEntity.PlayAnimation(ANIMATION_ID.ENTITY_DIE, new HurtAnimationProperties(new DamageLog(null, 0, 0, null)));
            StartCoroutine(WaitForAnimationEnd(animatableEntity, animation));
        }
    }
}
