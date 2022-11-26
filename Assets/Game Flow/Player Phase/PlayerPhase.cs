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
    [SerializeField] EnemyPhase enemyPhase;

    [SerializeField] bool isRunning;
    
    [InspectorButton("OnStartPhaseClicked")]
    public bool StartPlayerPhase;
    public event Action Finished;

    private void OnStartPhaseClicked()
    {
        isRunning = true;
        StartPlayerRound();
    }

    private void Awake()
    {
        pendingTurns.AllTurnsComplete += EndPlayerRound;
        
    }

    private void Start()
    {
        enemyPhase.Finished += StartPlayerRound;
    }

    void StartPlayerRound()
    {
        var planners = playerPlanInitializer.InitializePlanners();
        currentPlannerTracker.TrackPlanningPlayer(planners, IsRunningPlayerPhase);
        pendingTurns.TrackPendingTurns(planners);
    }

    bool IsRunningPlayerPhase()
    {
        return isRunning;
    }

    void EndPlayerRound()
    {
        isRunning = false;
    }
}