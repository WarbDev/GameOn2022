using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] Ease ease;
    [SerializeField] Ease squishEase;
    [SerializeField] float duration;
    [SerializeField] float jumpHeight;

    public void AnimateMovement(MoveLog log)
    {
        Sequence jumpSequence = DOTween.Sequence();
        Sequence squishSequence = DOTween.Sequence();
        
        jumpSequence.SetEase(ease);
        squishSequence.SetEase(squishEase);
        squishSequence.Pause();
        
        jumpSequence.Append(transform.DOMove(new Vector3((log.End.X + log.Start.X) / 2f, log.End.Y + jumpHeight), duration / 2));
        jumpSequence.Append(transform.DOMove(new Vector3(log.End.X, log.End.Y), duration / 2)).OnComplete(jumpComplete);

        void jumpComplete()
        {
            squishSequence.Play();
            squishSequence.Append(transform.DOScaleY(0.8f, 0.3f));
            squishSequence.Append(transform.DOScaleY(1f, 0.3f));
        }


        
    }
}
