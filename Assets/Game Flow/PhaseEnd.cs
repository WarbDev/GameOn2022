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

    [SerializeField] WaveRunner waveRunner;
    [SerializeField] WaveEnd waveEnd;

    [SerializeField] List<Phase> completedPhases = new();
    public event Action NewRound;
    public event Action NewPlayerPhase;

    public void EndPhase(Phase phase)
    {
        StartCoroutine(EndPhaseRoutine(phase));
    }

    IEnumerator EndPhaseRoutine(Phase phase)
    {
        bool isFinished = false;
        WinState winState = WinState.NONE;
        CheckWinCondition(ref isFinished, ref winState);
        yield return new WaitUntil(()=>isFinished);
        if (winState != WinState.NONE)
        {
            completedPhases.Clear();
            GoToWinState(winState);
            yield break;
        }


        CheckNewRound(phase);
    }

    void CheckWinCondition(ref bool isFinished, ref WinState winState)
    {
        // Check if an enemy is on the enemy line.
        if (LocationUtility.GetEnemiesInPositions(LocationUtility.GetColumn(0)).Count > 0)
        {
            winState = WinState.ENEMY_WIN;
        }

        // Check if no enemies are left and the wave has no batches to spawn.
        if (Entities.EnemyCollection.EntitiesSet.Count == 0 && waveRunner.NoMoreBatches())
        {
            winState = WinState.BEAT_WAVE;
            if (waveRunner.OnLastWave())
            {
                winState = WinState.VICTORY;
            }
        }

        isFinished = true;
        return;
    }

    void GoToWinState(WinState winState)
    {
        if (winState == WinState.ENEMY_WIN)
        {
            Debug.Log("FAILURE!!!!!!!!!!!!!!!!");
        }
        if (winState == WinState.VICTORY)
        {
            waveEnd.RunWaveEnd(-1);
            Debug.Log("WINNNNNNNNNNNNNNNNNN!");
        }
        if (winState == WinState.BEAT_WAVE)
        {
            waveEnd.RunWaveEnd(waveRunner.WaveNumber);
            Debug.Log("WAVE ENDEDEDEDEDEDED!");
        }
    }



    void CheckNewRound(Phase phase)
    {
        Action GoToPhaseEnd = () => EndPhase(Phase.ENEMY_PHASE);
        completedPhases.Add(phase);

        if (completedPhases.Contains(Phase.PLAYER_PHASE) && completedPhases.Contains(Phase.ENEMY_PHASE))
        {
            completedPhases.Clear();
            NewPlayerPhase?.Invoke();
            playerPhase.StartPlayerRound();
            NewRound?.Invoke();
        }
        else
        {
            if (phase == Phase.PLAYER_PHASE)
            {
                
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

public enum WinState
{
    NONE,
    ENEMY_WIN,
    BEAT_WAVE,
    VICTORY
}