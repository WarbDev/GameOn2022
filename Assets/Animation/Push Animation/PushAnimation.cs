using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAnimation : EntityAnimation<PushAnimationProperties>
{
    public override Sequence CurrentlyPlaying { get => currentlyPlaying; }
    public override event Action<EntityAnimation<PushAnimationProperties>> AnimationFinished;

    private Sequence currentlyPlaying;

    public override void Play(PushAnimationProperties animationProperties)
    {
        currentlyPlaying = DOTween.Sequence();

        Location distance = animationProperties.PushLog.MoveLog.End - animationProperties.PushLog.MoveLog.Start;
        float time = MathF.Sqrt(Mathf.Pow(distance.X, 2) + Mathf.Pow(distance.Y, 2));

        currentlyPlaying.Append(animationProperties.PushLog.MoveLog.Entity.transform.DOMove(LocationUtility.LocationToVector3(animationProperties.PushLog.MoveLog.Entity.Location), time/7f + .1f).SetEase(Ease.Linear)).OnComplete(onComplete);

        void onComplete()
        {
            AnimationFinished?.Invoke(this);
        }
    }
}

public class PushAnimationProperties : IAnimationProperties
{
    PushLog pushLog;

    public PushAnimationProperties(PushLog pushLog)
    {
        this.pushLog = pushLog;
    }

    public PushLog PushLog { get => pushLog;}
}
