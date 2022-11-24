using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HurtAnimation : EntityAnimation<HurtAnimationProperties>
{
    public override event Action<EntityAnimation<HurtAnimationProperties>> AnimationFinished;
    
    Sequence currentlyPlaying = null;
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    public override void Play(HurtAnimationProperties animationProperties)
    {
        throw new NotImplementedException();
    }
}


public class HurtAnimationProperties : IAnimationProperties
{
    DamageLog damageLog;

    public HurtAnimationProperties(DamageLog damageLog)
    {
        this.damageLog = damageLog;
    }
}