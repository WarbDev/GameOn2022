using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStunned : EnemyAction
{
    public override event Action<EnemyAction> ActionFinished;
    public override void DoEnemyAction()
    {
        var animation = Animatable.PlayAnimation<StunAnimationProperties>(ANIMATION_ID.ENEMY_STUNNED, new());

        animation.AnimationFinished += animationFinished;
        void animationFinished(EntityAnimation<StunAnimationProperties> animation)
        {
            animation.AnimationFinished -= animationFinished;
            ActionFinished?.Invoke(this);
        }
    }
}
