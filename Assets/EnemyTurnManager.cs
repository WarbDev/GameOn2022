using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnManager : MonoBehaviour
{
    [SerializeField] RunEnemyMovement MovementCalculation;
    [SerializeField] RunEnemyMovementAnimation MovementAnimator;

    void Awake()
    {
        MovementCalculation.Finished += RunAnimation;
    }

    void RunAnimation()
    {
        MovementAnimator.Play();
        StartCoroutine(DoEnemyAttackOnceAnimationFinished());
    }

    IEnumerator DoEnemyAttackOnceAnimationFinished()
    {
        while (MovementAnimator.IsPlaying)
        {
            yield return null;
        }


    }
}
