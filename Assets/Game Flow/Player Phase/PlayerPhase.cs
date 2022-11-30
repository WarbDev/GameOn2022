using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Manages the Player Phase state.
/// </summary>
public class PlayerPhase : MonoBehaviour
{

    [SerializeField] PendingTurns pendingTurns;
    [SerializeField] TurnPlans playerPlanInitializer;
    [SerializeField] CurrentlyPlanningPlayer currentPlannerTracker;
    [SerializeField] PhaseEnd phaseEnd;

    public event Action DeathAnimationTick;
    public event Action<bool> AnyPlayerNowPlanning;

    [SerializeField] bool isRunning;

    private void Awake()
    {
        pendingTurns.AllTurnsComplete += EndPlayerRound;
    }

    public void StartPlayerRound()
    {
        isRunning = true;
        var planners = playerPlanInitializer.InitializePlanners();
        currentPlannerTracker.TrackPlanningPlayer(planners, IsRunningPlayerPhase);
        currentPlannerTracker.TriedAction += TickDeathAnimation;
        currentPlannerTracker.AnyPlayerNowPlanning += BroadcastAnyPlayerNowPlanning;
        pendingTurns.TrackPendingTurns(planners);
    }

    bool IsRunningPlayerPhase()
    {
        return isRunning;
    }

    void EndPlayerRound()
    {
        isRunning = false;
        StartCoroutine(EndRoundRoutine());
        IEnumerator EndRoundRoutine()
        {
            
            yield return new WaitForSeconds(1f);
            currentPlannerTracker.TriedAction -= TickDeathAnimation;
            currentPlannerTracker.AnyPlayerNowPlanning -= BroadcastAnyPlayerNowPlanning;
            phaseEnd.EndPhase(Phase.PLAYER_PHASE);
        }

        
    }

    void TickDeathAnimation()
    {
        DeathAnimationTick?.Invoke();
    }

    void BroadcastAnyPlayerNowPlanning(bool b)
    {
        AnyPlayerNowPlanning?.Invoke(b);
    }
}