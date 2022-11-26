using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnComponent : MonoBehaviour
{
    [SerializeField] PlayerTurnMovement movement;
    [SerializeField] PlayerTurnAction action;

    [SerializeField] TurnPlanningInput input;

    public PLAN_STATE STATE = PLAN_STATE.AWAITING;

    bool hasPlannedMovement;
    bool hasPlannedAction;

    private void Start()
    {

    }

    public void BeginNewRound()
    {
        hasPlannedAction = false;
        hasPlannedMovement = false;

        StartCoroutine(AwaitingSelection());
    }

    IEnumerator AwaitingSelection()
    {
        STATE = PLAN_STATE.AWAITING;
        input.MovementSelected += TryEnterMovementState;

        while (STATE == PLAN_STATE.AWAITING)
        {
            yield return null;
            if (!CanDoAction() && !CanDoMovement())
            {
                STATE = PLAN_STATE.ASLEEP;
            }
        }

        input.MovementSelected -= TryEnterMovementState;

        void TryEnterMovementState()
        {
            if (CanDoMovement())
            {
                STATE = PLAN_STATE.PLAN_MOVEMENT;
                StartCoroutine(PlanningMovement());
            }
        }

        
    }

    IEnumerator PlanningMovement()
    {
        STATE = PLAN_STATE.PLAN_MOVEMENT;
        movement.DidMovement += OnMovementCallback;
        movement.PlanMovement();

        while (STATE == PLAN_STATE.PLAN_MOVEMENT)
        {
            yield return null;
        }

        movement.DidMovement -= OnMovementCallback;

        void OnMovementCallback(bool success)
        {
            if (success)
            {
                hasPlannedMovement = success;
                STATE = PLAN_STATE.AWAITING;
                StartCoroutine(AwaitingSelection());
            }
        }
    }

    IEnumerator PlanningAction()
    {
        yield return null;
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
        return (STATE == PLAN_STATE.ASLEEP);
    }

    public enum PLAN_STATE
    {
        AWAITING,
        PLAN_MOVEMENT,
        DOING_ACTION,
        ASLEEP
    }
}