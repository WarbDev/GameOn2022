using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartIdleOnPlayerPhaseBegin : MonoBehaviour
{
    [SerializeField] AnimatableEntity animatableEntity;
    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start()
    {
        GameFlow.NewPlayerPhase += PlayIdle;
    }

    private void OnDestroy()
    {
        GameFlow.NewPlayerPhase -= PlayIdle;
    }

    void PlayIdle()
    {
        if (animatableEntity != null)
        {
            animatableEntity.PlayAnimation<SpriteAnimationProperties>(ANIMATION_ID.ENTITY_IDLE, new(spriteRenderer));
        }
    }
}
