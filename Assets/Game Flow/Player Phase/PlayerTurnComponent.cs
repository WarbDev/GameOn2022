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

    PLAN_STATE STATE = PLAN_STATE.AWAITING;
    public event Action<PlayerTurnComponent, PLAN_STATE> StateChanged;

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
            
            if (CurrentlyPlanningPlayer.CurrentlyPlanning != null &&
                CurrentlyPlanningPlayer.CurrentlyPlanning != this)
            {
                STATE = PLAN_STATE.LOCKED;
                StateChanged?.Invoke(this, STATE);
                StartCoroutine(LockedState());
            }

            else if (!CanDoAction() && !CanDoMovement())
            {
                STATE = PLAN_STATE.ASLEEP;
                StateChanged?.Invoke(this, STATE);
            }
            yield return null;
        }

        input.MovementSelected -= TryEnterMovementState;
        input.ActionSelected -= TryEnterActionState;

        void TryEnterMovementState()
        {
            if (CanDoMovement())
            {
                STATE = PLAN_STATE.PLAN_MOVEMENT;
                StateChanged?.Invoke(this, STATE);
                StartCoroutine(PlanningMovement());
            }
        }

        void TryEnterActionState(Move move)
        {
            if (CanDoAction(move))
            {
                STATE = PLAN_STATE.DOING_ACTION;
                StateChanged?.Invoke(this, STATE);
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
            StateChanged?.Invoke(this, STATE);
            StartCoroutine(AwaitingSelection());
        }
    }

    IEnumerator PlanningAction(Move move)
    {

        STATE = PLAN_STATE.DOING_ACTION;
        action.DidAction += OnActionCallback;
        action.PlanAction(move);
        while (STATE == PLAN_STATE.DOING_ACTION)
        {
            yield return null;
        }
        action.DidAction -= OnActionCallback;

        void OnActionCallback(bool success)
        {
            hasPlannedAction = success;
            STATE = PLAN_STATE.AWAITING;
            StateChanged?.Invoke(this, STATE);
            StartCoroutine(AwaitingSelection());
        }
    }

    IEnumerator LockedState()
    {
        while (STATE == PLAN_STATE.LOCKED && CurrentlyPlanningPlayer.CurrentlyPlanning != null &&
                CurrentlyPlanningPlayer.CurrentlyPlanning != this)
        {
            yield return null;
        }
        STATE = PLAN_STATE.AWAITING;
        StateChanged?.Invoke(this, STATE);
        StartCoroutine(AwaitingSelection());
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
        ASLEEP,
        LOCKED
    }

    public static HashSet<PLAN_STATE> ActiveStates =
        new HashSet<PLAN_STATE> { PLAN_STATE.PLAN_MOVEMENT, PLAN_STATE.DOING_ACTION };
}