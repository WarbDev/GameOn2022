using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeathAnimationTicker : MonoBehaviour
{
    [SerializeField] EnemyPhase enemyPhase;
    [SerializeField] PlayerPhase playerPhase;
    [SerializeField] PhaseEnd phaseEnd;
    public event Action DeathAnimationTick;

    void Start()
    {
        phaseEnd.DeathAnimationTick += InvokeDA;
        enemyPhase.DeathAnimationTick += InvokeDA;
        playerPhase.DeathAnimationTick += InvokeDA;
    }

    private void OnDestroy()
    {
        phaseEnd.DeathAnimationTick -= InvokeDA;
        enemyPhase.DeathAnimationTick -= InvokeDA;
        playerPhase.DeathAnimationTick -= InvokeDA;
    }

    void InvokeDA()
    {
        DeathAnimationTick?.Invoke();
    }
}
