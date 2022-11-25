using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnComponent : MonoBehaviour
{
    private bool hasMadeTurn = false;
    public bool HasMadeTurn { get => hasMadeTurn; }
    
    public void BeginNewRound()
    {
        hasMadeTurn = false;
    }

    /// <summary>
    /// Does this player still need their turn to be planned?
    /// </summary>
    public bool IsTurnPending()
    {
        return hasMadeTurn;
    }

}
