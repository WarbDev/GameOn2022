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
    
    [InspectorButton("OnStartPhaseClicked")]
    public bool StartPlayerPhase;

    private void OnStartPhaseClicked()
    {
        StartPlayerRound();
    }

    private void Awake()
    {
        pendingTurns.AllTurnsComplete += EndPlayerRound;
    }

    void StartPlayerRound()
    {
        pendingTurns.TrackPendingTurns(playerPlanInitializer.InitializePlanners());
    }

    void EndPlayerRound()
    {

    }
}