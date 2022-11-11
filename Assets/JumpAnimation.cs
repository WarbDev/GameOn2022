using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpAnimation : EntityAnimation<JumpAnimationProperties>
{
    [SerializeField] Ease ease;
    [SerializeField] Ease squishEase;
    [SerializeField] float duration;
    [SerializeField] float jumpHeight;
    [SerializeField] Transform affectedTransform;
    [SerializeField] SpriteRenderer targetSprite;
    Sequence currentlyPlaying;


    private void Start()
    {
        enabled = false;
    }

    private void Update()
    {
        
    }

    public override void Pause()
    {
        enabled = false;
        if (currentlyPlaying != null)
        {
            currentlyPlaying.Pause();
        }
    }

    public override void Play(JumpAnimationProperties properties)
    {
        enabled = true;
        Sequence jumpSequence = DOTween.Sequence();
        Sequence squishSequence = DOTween.Sequence();

        jumpSequence.SetEase(ease);
        squishSequence.SetEase(squishEase);
        squishSequence.Pause();

        currentlyPlaying = jumpSequence;
        jumpSequence.Append(affectedTransform.DOMove(new Vector3((properties.StartPosition.x + properties.EndPosition.x) / 2f, properties.EndPosition.y + jumpHeight), duration / 2));
        jumpSequence.Append(affectedTransform.DOMove(new Vector3(properties.EndPosition.x, properties.EndPosition.y), duration / 2)).OnComplete(jumpComplete);

        void jumpComplete()
        {
            currentlyPlaying = squishSequence;
            squishSequence.Play();
            squishSequence.Append(affectedTransform.DOScaleY(0.8f, 0.3f));
            squishSequence.Append(affectedTransform.DOScaleY(1f, 0.3f));
        }

    }

    public override void Unpause()
    {
        enabled = true;
        if (currentlyPlaying != null)
        {
            currentlyPlaying.Play();
        }
    }
}

public class JumpAnimationProperties : IAnimationProperties
{
    Vector3 startPosition;
    Vector3 endPosition;

    public Vector3 StartPosition { get => startPosition; }
    public Vector3 EndPosition { get => endPosition; }
    
    public JumpAnimationProperties(Vector3 startPosition, Vector3 endPosition)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
    }
}