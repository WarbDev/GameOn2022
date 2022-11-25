using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Manages the Player Round state.
/// </summary>
public class PlayerRound : MonoBehaviour
{
    public event Action Begin;
    public event Action End;
    [SerializeField] PendingTurns pendingTurns;

    private void Awake()
    {
        pendingTurns.AllTurnsComplete += EndPlayerRound;
    }

    void StartPlayerRound()
    {
        Begin?.Invoke();
        pendingTurns.TrackPendingTurns();
    }

    void EndPlayerRound()
    {
        End?.Invoke();
    }
}