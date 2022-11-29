using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeLightning : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    [SerializeField] int timesCharged;
    public int TimesCharged { get => timesCharged; }

    [SerializeField] public int ChargeRequired = 2;

    public void ResetCharges()
    {
        timesCharged = 0;
    }

    public override void DoEnemyAction()
    {
        var animation = Animatable.PlayAnimation<ChargeAnimationProperties>(ANIMATION_ID.ENEMY_CHARGE, new(timesCharged));

        timesCharged++;

        animation.AnimationFinished += animationFinished;
        void animationFinished(EntityAnimation<ChargeAnimationProperties> animation)
        {
            animation.AnimationFinished -= animationFinished;
            ActionFinished?.Invoke(this);
        }
    }
}
