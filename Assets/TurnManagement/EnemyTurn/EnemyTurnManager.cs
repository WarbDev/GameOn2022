using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnManager : MonoBehaviour
{
    [SerializeField] RunEnemyMovement MovementCalculation;
    [SerializeField] RunEnemyMovementAnimation MovementAnimator;

    void RunEnemyTurn()
    {
        // Calculate movement
        // Play movement animations

        // Calculate actions
        // Play action animations

        // Spawn in new wave
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
