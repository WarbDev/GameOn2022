using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnComponent : MonoBehaviour
{
    [SerializeField] PlayerTurnMovement movement;

    bool hasPlannedMovement;
    bool hasPlannedAction;

    private void Start()
    {
        movement.DidMovement += OnDidMovement;
    }

    public void BeginNewRound()
    {
        hasPlannedAction = false;
        hasPlannedMovement = false;
    }

    void OnDidMovement(bool success)
    {
        hasPlannedMovement = success;
    }

    public bool CanDoMovement()
    {
        return !hasPlannedMovement;
    }

    public bool CanDoAction()
    {
        return !hasPlannedAction;
    }

    /// <summary>
    /// Does this player still need their turn to be planned?
    /// </summary>
    public bool IsTurnPending()
    {
        return !(CanDoAction() || CanDoMovement());
    }
}
