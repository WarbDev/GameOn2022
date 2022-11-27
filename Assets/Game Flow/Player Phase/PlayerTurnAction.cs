using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnAction : MonoBehaviour
{
    [SerializeField] Player player;
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
            planningMove = null;
        }
    }

    public void StopPlanning()
    {
        throw new NotImplementedException();
        if (planningMove != null)
        {
            DidAction?.Invoke(false);
            // cancel planning move
        }
    }

    
}