using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Container for various game flow events. UNSUBSCRIBE FROM THESE ONDESTROY.
/// </summary>
public class GameFlow : MonoBehaviour
{
    [SerializeField] PhaseEnd phaseEnd;

    /// <summary>
    /// Invoked when a new player phase has started.
    /// </summary>
    public static event Action NewPlayerPhase;

    /// <summary>
    /// Invoked when a new round has started.
    /// </summary>
    public static event Action NewRound;

    private void Start()
    {
        phaseEnd.NewRound += (() => NewRound?.Invoke());
    }
}