using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour, IBroadcastMove
{
    public event Action<Move> Broadcast;
    [SerializeField] Move move;

    public void Clicked()
    {
        Broadcast?.Invoke(move);
    }
}