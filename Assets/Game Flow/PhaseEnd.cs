using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Is entered whenever an enemy or player phase ends.
/// Checks to see if an entire round has been completed, otherwise, moves to the opposing phase.
/// FEATURE REQUEST: Should check to see if the game should end
/// </summary>
public class PhaseEnd : MonoBehaviour
{
    [SerializeField] EnemyPhase enemyPhase;
    [SerializeField] PlayerPhase playerPhase;

    [SerializeField] List<Phase> completedPhases = new();
    public event Action NewRound;
    public event Action NewPlayerPhase;

    public void EndPhase(Phase phase)
    {
        CheckNewRound(phase);
    }

    void CheckNewRound(Phase phase)
    {
        completedPhases.Add(phase);

        if (completedPhases.Contains(Phase.PLAYER_PHASE) && completedPhases.Contains(Phase.ENEMY_PHASE))
        {
            completedPhases.Clear();
            NewRound?.Invoke();
        }
        else
        {
            if (phase == Phase.PLAYER_PHASE)
            {
                Action GoToPhaseEnd = () => CheckNewRound(Phase.ENEMY_PHASE);
                enemyPhase.StartRound(GoToPhaseEnd);
            }
            else if (phase == Phase.ENEMY_PHASE)
            {
                NewPlayerPhase?.Invoke();
                playerPhase.StartPlayerRound();
            }
        }
    }
}

public enum Phase
{
    PLAYER_PHASE,
    ENEMY_PHASE
}
