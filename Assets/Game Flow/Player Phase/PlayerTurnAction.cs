using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnAction : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] PlayerActions playerActions;
    public event Action<bool> DidAction;

    Move planningMove = null;

    public void PlanAction(Move move)
    {
        planningMove = move;
        move.MoveCompleted += OnMoveCompletion;
        move.DoMove(player);

        void OnMoveCompletion(bool success)
        {
            DidAction?.Invoke(success);
            move.MoveCompleted -= OnMoveCompletion;
            if (success) playerActions.UsedMove(move);
            planningMove = null;
        }
    }

    
}