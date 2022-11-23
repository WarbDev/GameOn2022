using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnComponent : MonoBehaviour
{
    private bool hasMadeTurn;
    public bool HasMadeTurn { get => hasMadeTurn; }

    private void Start()
    {
        hasMadeTurn = false;
    }

    public void TurnComplete()
    {
        hasMadeTurn = true;
    }

    public bool CanIMakeATurn()
    {
        return hasMadeTurn;
    }

}
