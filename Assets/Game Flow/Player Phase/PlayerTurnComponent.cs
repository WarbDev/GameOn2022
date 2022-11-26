using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnComponent : MonoBehaviour
{
    [SerializeField] PlayerTurnMovement movement;
    [SerializeField] PlayerTurnAction action;
    [SerializeField] PlayerActions actionPool;

    [SerializeField] TurnPlanningInput input;

    public PLAN_STATE STATE = PLAN_STATE.AWAITING;

    bool hasPlannedMovement;
    bool hasPlannedAction;

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
        input.ActionSelected += TryEnterActionState;

        while (STATE == PLAN_STATE.AWAITING)
        {
            yield return null;
            if (!CanDoAction() && !CanDoMovement())
            {
                STATE = PLAN_STATE.ASLEEP;
            }
        }

        input.MovementSelected -= TryEnterMovementState;
        input.ActionSelected -= TryEnterActionState;

        void TryEnterMovementState()
        {
            if (CanDoMovement())
            {
                STATE = PLAN_STATE.PLAN_MOVEMENT;
                StartCoroutine(PlanningMovement());
            }
        }

        void TryEnterActionState(Move move)
        {
            if (CanDoAction(move))
            {
                STATE = PLAN_STATE.DOING_ACTION;
                StartCoroutine(PlanningAction(move));
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
            hasPlannedMovement = success;
            STATE = PLAN_STATE.AWAITING;
            StartCoroutine(AwaitingSelection());
        }
    }

    IEnumerator PlanningAction(Move move)
    {

        STATE = PLAN_STATE.DOING_ACTION;
        action.DidAction += OnActionCallback;
        action.PlanAction(move);
        while(STATE == PLAN_STATE.DOING_ACTION)
        {
            yield return null;
        }
        action.DidAction -= OnActionCallback;

        void OnActionCallback(bool success)
        {
            hasPlannedAction = success;
            STATE = PLAN_STATE.AWAITING;
            StartCoroutine(AwaitingSelection());
        }
    }

    public bool CanDoMovement()
    {
        return !hasPlannedMovement;
    }

    public bool CanDoAction()
    {
        return !hasPlannedAction;
    }

    public bool CanDoAction(Move move)
    {
        return CanDoAction() && actionPool.CanDo(move);
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