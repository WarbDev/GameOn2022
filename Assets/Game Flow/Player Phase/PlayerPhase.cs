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
        pendingTurns.TrackPendingTurns(planners);
    }

    bool IsRunningPlayerPhase()
    {
        return isRunning;
    }

    void EndPlayerRound()
    {
        isRunning = false;
        currentPlannerTracker.TriedAction -= TickDeathAnimation;
        phaseEnd.EndPhase(Phase.PLAYER_PHASE);
    }

    void TickDeathAnimation()
    {
        DeathAnimationTick?.Invoke();
    }
}