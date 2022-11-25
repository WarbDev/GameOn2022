using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnComponent : MonoBehaviour
{
    [SerializeField] PlayerTurnMovement movement;

    private bool hasMadeTurn = false;
    
    public void BeginNewRound()
    {
        StartCoroutine(PlayerTurnRoutine());
    }

    IEnumerator PlayerTurnRoutine()
    {
        hasMadeTurn = false;
        
        yield return null;
    }

    /// <summary>
    /// Does this player still need their turn to be planned?
    /// </summary>
    public bool IsTurnPending()
    {
        return hasMadeTurn;
    }

}
