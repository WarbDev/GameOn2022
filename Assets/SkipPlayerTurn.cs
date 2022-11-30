using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipPlayerTurn : MonoBehaviour
{
    public event Action RequestedPlayerSkip;
    public void SkipPlayerTurns()
    {
        RequestedPlayerSkip?.Invoke();
    }
}
