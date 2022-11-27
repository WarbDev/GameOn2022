using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnd : MonoBehaviour
{
    [SerializeField] EnemyPhase enemyPhase;
    [SerializeField] PlayerPhase playerPhase;

    public void EndedPlayerPhase()
    {
        enemyPhase.StartRound();
    }
}
