using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnAction : MonoBehaviour
{
    [SerializeField] Player player;
    public event Action<bool> DidAction;

    public void PlanAction(Move move)
    {
        move.MoveCompleted += OnMoveCompletion;
        move.DoMove(player);

        void OnMoveCompletion(bool success)
        {
            DidAction?.Invoke(success);
            move.MoveCompleted -= OnMoveCompletion;
        }
    }

    
}