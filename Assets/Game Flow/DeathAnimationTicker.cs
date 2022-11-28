using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathAnimationTicker : MonoBehaviour
{
    [SerializeField] EnemyPhase enemyPhase;
    [SerializeField] PlayerPhase playerPhase;
    public event Action DeathAnimationTick;

    void Start()
    {
        enemyPhase.DeathAnimationTick += InvokeDA;
        playerPhase.DeathAnimationTick += InvokeDA;
    }

    private void OnDestroy()
    {
        enemyPhase.DeathAnimationTick -= InvokeDA;
        playerPhase.DeathAnimationTick -= InvokeDA;
    }

    void InvokeDA()
    {
        DeathAnimationTick?.Invoke();
    }
}
